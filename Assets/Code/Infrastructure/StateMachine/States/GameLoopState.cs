using System;
using Code.Extensions;
using Code.Infrastructure.Factories;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState, IDisposable
    {
        private readonly IGameFactory _gameFactory;
        private IGameStateMachine _stateMachine;

        public GameLoopState(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            _gameFactory.GamePlayUI.AgainButton.Add(OnAgain);
        }

        public void Exit()
        {
            _gameFactory.GamePlayUI.AgainButton.Remove(OnAgain);
        }

        public void Dispose()
        {
            _gameFactory.GamePlayUI.AgainButton.RemoveAll();
        }

        private void OnAgain()
        {
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }
    }
}