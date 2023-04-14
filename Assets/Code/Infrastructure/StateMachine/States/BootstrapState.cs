using Code.Infrastructure.Services.Progress;

namespace Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IDefaultState
    {
        private readonly IProgressService _progressService;

        private IGameStateMachine _stateMachine;

        public BootstrapState(IProgressService progressService) =>
            _progressService = progressService;

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            _progressService.Load();
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        public void Exit()
        {
        }
    }
}