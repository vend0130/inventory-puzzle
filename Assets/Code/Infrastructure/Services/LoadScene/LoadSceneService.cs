using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Services.LoadScene
{
    public class LoadSceneService : ILoadSceneService
    {
        private readonly ICurtain _curtain;

        private bool _firstLoadIsEnded;

        public LoadSceneService(ICurtain curtain) =>
            _curtain = curtain;

        public async UniTask CurtainOnAsync()
        {
            if (!_firstLoadIsEnded)
                await FirstLoadAsync();
            else
                await _curtain.FadeOn();
        }

        public async UniTask LoadSceneAsync(string name) =>
            await LoadScene(name);

        public async UniTask CurtainOffAsync() =>
            await _curtain.FadeOff();

        private async UniTask FirstLoadAsync()
        {
            _firstLoadIsEnded = true;
            _curtain.On();
            await UniTask.Yield();
        }

        private async UniTask LoadScene(string sceneName)
        {
            var load = SceneManager.LoadSceneAsync(sceneName);

            while (!load.isDone)
                await UniTask.Yield();
        }
    }
}