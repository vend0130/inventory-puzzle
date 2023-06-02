using Code.Data;
using Code.Infrastructure.Services.Audio;
using Code.Infrastructure.Services.Progress;
using Code.Infrastructure.Services.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IDefaultState
    {
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAudioService _audioService;

        private IGameStateMachine _stateMachine;

        public BootstrapState(IProgressService progressService, ISaveLoadService saveLoadService,
            IAudioService audioService)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            Application.targetFrameRate = 60;

            Init().Forget();
        }

        public void Exit()
        {
        }

        private async UniTaskVoid Init()
        {
            ProgressData data = await LoadProgress();
            _progressService.ChangeProgressData(data);

            _audioService.Init(_progressService.ProgressData.Sound);
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);
        }

        private async UniTask<ProgressData> LoadProgress()
        {
            ProgressData data = await _saveLoadService.Load();
            return data ?? new ProgressData();
        }
    }
}