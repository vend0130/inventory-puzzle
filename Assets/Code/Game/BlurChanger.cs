using System.Threading;
using Cysharp.Threading.Tasks;
using LeTai.Asset.TranslucentImage;
using UnityEngine;

namespace Code.Game
{
    public class BlurChanger : MonoBehaviour
    {
        [SerializeField] private TranslucentImageSource _translucent;

        private const int MillisecondsDelay = 1000;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private void Start() =>
            RenderAsync().Forget();

        private void OnDestroy() =>
            DisposeToken();

        public void Render()
        {
            if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
                return;

            DisposeToken();
            _tokenSource = new CancellationTokenSource();
            RenderAsync().Forget();
        }

        //note: hack for updated TranslucentImageSource
        private async UniTask RenderAsync()
        {
            _translucent.maxUpdateRate = 60;
            await UniTask.Delay(MillisecondsDelay, cancellationToken: _tokenSource.Token);
            _translucent.maxUpdateRate = 0;

            _tokenSource?.Cancel();
        }

        private void DisposeToken()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }
    }
}