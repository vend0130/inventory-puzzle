using System;
using Code.Extensions;
using Code.Game.Item.Items;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services.Progress;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState, IDisposable
    {
        private readonly IGameFactory _gameFactory;
        private readonly IProgressService _progressService;

        private IGameStateMachine _stateMachine;

        public GameLoopState(IGameFactory gameFactory, IProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            _gameFactory.GamePlayUI.AgainButton.Add(OnAgain);
            _gameFactory.GamePlayUI.ExitButton.Add(OnExit);

            _gameFactory.ItemMenu.CreateItemHandler += CreateItem;
            _gameFactory.InventoryGame.AllItemsInInventoryHandler += EndGame;
        }

        public void Exit()
        {
            _gameFactory.GamePlayUI.AgainButton.Remove(OnAgain);
            _gameFactory.GamePlayUI.ExitButton.Remove(OnExit);

            _gameFactory.ItemMenu.CreateItemHandler -= CreateItem;
            _gameFactory.InventoryGame.AllItemsInInventoryHandler -= EndGame;
        }

        public void Dispose() =>
            _gameFactory.GamePlayUI.AgainButton.RemoveAll();

        private void CreateItem(BaseItem parentItem, int index)
        {
            BaseItem item = _gameFactory.CreateItem(parentItem, index, _gameFactory.PointerHandler.MousePosition);
            _gameFactory.DragItems.AddSpawnedItem(item);
            _gameFactory.PointerHandler.SetMouseDrag();
        }

        private void EndGame()
        {
            _progressService.NextLevel();
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        private void OnAgain() =>
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);

        private void OnExit() =>
            _stateMachine.Enter<ExitState>();
    }
}