using Code.Infrastructure.Services.LoadScene;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadSceneState : IPayloadState<string>
    {
        private readonly ILoadSceneService _loadSceneService;

        public LoadSceneState(ILoadSceneService loadSceneService)
        {
            _loadSceneService = loadSceneService;
        }

        public async void Enter(string sceneName)
        {
            await _loadSceneService.CurtainOnAsync();
            await _loadSceneService.LoadSceneAsync(sceneName);
            await _loadSceneService.CurtainOffAsync(); //TODO: to Exit()
        }

        public void Exit()
        {
        }
    }
}