using System;
using System.Threading;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services.Ad;
using Code.Infrastructure.Services.LoadScene;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadSceneState : IPayloadState<string>, IDisposable
    {
        private readonly ILoadSceneService _loadSceneService;
        private readonly IGameFactory _gameFactory;
        private readonly IAdService _adService;

        private CancellationTokenSource _tokenSource;
        private IGameStateMachine _stateMachine;

        public LoadSceneState(ILoadSceneService loadSceneService, IGameFactory gameFactory, IAdService adService)
        {
            _loadSceneService = loadSceneService;
            _gameFactory = gameFactory;
            _adService = adService;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public async void Enter(string sceneName)
        {
            _adService.Show();
            
            _tokenSource = new CancellationTokenSource();

            await _loadSceneService.CurtainOnAsync();
            await _loadSceneService.LoadSceneAsync(sceneName);
            await CreateObjects();
            await _loadSceneService.CurtainOffAsync();

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit() =>
            DisposeToken();

        public void Dispose() =>
            DisposeToken();

        private async UniTask CreateObjects()
        {
            _gameFactory.CreateBackground();
            _gameFactory.CreateInfoPanel();
            _gameFactory.CreateGamePlayUI();
            _gameFactory.CreateLevel();

            await UniTask.Yield(cancellationToken: _tokenSource.Token);
        }

        private void DisposeToken()
        {
            if (_tokenSource == null)
                return;

            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}