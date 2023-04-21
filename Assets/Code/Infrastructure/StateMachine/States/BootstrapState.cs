using Code.Infrastructure.Services.Audio;
using Code.Infrastructure.Services.Progress;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IDefaultState
    {
        private readonly IProgressService _progressService;
        private readonly IAudioService _audioService;

        private IGameStateMachine _stateMachine;

        public BootstrapState(IProgressService progressService, IAudioService audioService)
        {
            _progressService = progressService;
            _audioService = audioService;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            Application.targetFrameRate = 60;

            _progressService.Load();
            _audioService.Init();
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        public void Exit()
        {
        }
    }
}