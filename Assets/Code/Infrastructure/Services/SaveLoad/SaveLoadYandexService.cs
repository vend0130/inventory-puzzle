using System;
using System.Threading;
using Code.Data;
using Code.Extensions;
using Code.Infrastructure.Services.Progress;
using Cysharp.Threading.Tasks;
using Plugins.Yandex;

namespace Code.Infrastructure.Services.SaveLoad
{
    public class SaveLoadYandexService : ISaveLoadService, IDisposable
    {
        private readonly IProgressService _progressService;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        private string _data;

        public SaveLoadYandexService(IProgressService progressService)
        {
            _progressService = progressService;
            YandexManager.LoadedHandler += Loaded;
        }

        public void Dispose()
        {
            YandexManager.LoadedHandler -= Loaded;
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }

        public void Save() =>
            YandexManager.CallSave(_progressService.ProgressData.ToJson());

        public async UniTask<ProgressData> Load()
        {
            _data = null;
            YandexManager.CallLoad();
            await UniTask.WaitUntil(() => _data != null, cancellationToken: _cancellation.Token);
            return _data.ToDeserialized<ProgressData>();
        }

        private void Loaded(string data) =>
            _data = data;
    }
}