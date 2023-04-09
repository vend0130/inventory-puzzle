using System;
using System.Threading;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services.LoadScene;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadSceneState : IPayloadState<string>, IDisposable
    {
        private readonly ILoadSceneService _loadSceneService;
        private readonly IGameFactory _gameFactory;

        private CancellationTokenSource _tokenSource;
        private IGameStateMachine _stateMachine;

        public LoadSceneState(ILoadSceneService loadSceneService, IGameFactory gameFactory)
        {
            _loadSceneService = loadSceneService;
            _gameFactory = gameFactory;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public async void Enter(string sceneName)
        {
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