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

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private void Start()
        {
            _translucent.maxUpdateRate = 60;
            Render().Forget();
        }

        private void OnDestroy()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        //note: hack for updated TranslucentImageSource
        private async UniTask Render()
        {
            await UniTask.Delay(MillisecondsDelay, cancellationToken: _tokenSource.Token);
            _translucent.maxUpdateRate = 0;
        }
    }
}