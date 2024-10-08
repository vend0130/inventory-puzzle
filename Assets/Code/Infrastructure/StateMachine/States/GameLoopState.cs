﻿using System;
using System.Threading;
using Code.Data.Audio;
using Code.Extensions;
using Code.Game.Item.Items;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services.Audio;
using Code.Infrastructure.Services.Progress;
using Code.Infrastructure.Services.SaveLoad;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState, IDisposable
    {
        private const int DelayMilliseconds = 500;

        private readonly IGameFactory _gameFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progressService;
        private readonly IAudioService _audioService;

        private IGameStateMachine _stateMachine;
        private CancellationTokenSource _tokenSource;

        public GameLoopState(IGameFactory gameFactory, ISaveLoadService saveLoadService,
            IProgressService progressService, IAudioService audioService)
        {
            _gameFactory = gameFactory;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _audioService = audioService;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            _gameFactory.GamePlayUI.ExitButton.Add(OnExit);

            _gameFactory.ItemMenu.CreateItemHandler += CreateItem;
            _gameFactory.InventoryGame.AllItemsInInventoryHandler += EndGame;

            _gameFactory.SimplyMenu.PlayLevelHandler += PlayLevel;
            _gameFactory.SimplyMenu.AgainButton.ClickHandler += Again;
            _gameFactory.SimplyMenu.SoundButton.ClickHandler += SwitchSound;

            _tokenSource = new CancellationTokenSource();
        }

        public void Exit()
        {
            _gameFactory.GamePlayUI.ExitButton.Remove(OnExit);

            _gameFactory.ItemMenu.CreateItemHandler -= CreateItem;
            _gameFactory.InventoryGame.AllItemsInInventoryHandler -= EndGame;

            _gameFactory.SimplyMenu.PlayLevelHandler -= PlayLevel;
            _gameFactory.SimplyMenu.AgainButton.ClickHandler -= Again;
            _gameFactory.SimplyMenu.SoundButton.ClickHandler -= SwitchSound;

            DisposeToken();
        }

        public void Dispose()
        {
            _gameFactory.GamePlayUI.ExitButton.RemoveAll();

            DisposeToken();
        }

        private void DisposeToken()
        {
            if (_tokenSource == null)
                return;

            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        private void CreateItem(BaseItem parentItem, int index)
        {
            BaseItem item = _gameFactory.CreateItem(parentItem, index, _gameFactory.PointerHandler.MousePosition);
            _gameFactory.DragItems.AddSpawnedItem(item);
            _gameFactory.PointerHandler.SetMouseDrag();
            _gameFactory.InventoryGame.AddItem(item);
        }

        private void EndGame()
        {
            _audioService.Play(SoundType.Win);
            _progressService.NextLevel();
            _saveLoadService.Save();
            Delay().Forget();
        }

        private async UniTask Delay()
        {
            await UniTask.Delay(DelayMilliseconds, cancellationToken: _tokenSource.Token);
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        private void PlayLevel(int level)
        {
            _progressService.ChangeLevel(level);
            _saveLoadService.Save();
            _audioService.Play(SoundType.Button);
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        private void Again()
        {
            _audioService.Play(SoundType.Button);
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        private void SwitchSound()
        {
            _audioService.ChangeEffectState();
            _audioService.Play(SoundType.Button);
            _gameFactory.SimplyMenu.SoundButton.ChangeState(_audioService.EffectsState);

            _progressService.ProgressData.ChangeSoundState(_audioService.EffectsState);
            _saveLoadService.Save();
        }

        private void OnExit()
        {
            _audioService.Play(SoundType.Button);
            _stateMachine.Enter<ExitState>();
        }
    }
}