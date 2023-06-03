using System;
using System.Threading;
using Code.Data;
using Code.Extensions;
using Code.Infrastructure.Services.Progress;
using Cysharp.Threading.Tasks;
using Plugins.Yandex;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoad
{
    public class SaveLoadYandexService : ISaveLoadService, IDisposable
    {
        private readonly IProgressService _progressService;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        private bool _isLoaded = false;
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
            new GameObject(nameof(DataResetHelper)).AddComponent<DataResetHelper>();
            
            _data = null;
            _isLoaded = false;

            YandexManager.CallLoad();

            await UniTask.WaitUntil(() => _isLoaded, cancellationToken: _cancellation.Token);
            return _data.ToDeserialized<ProgressData>();
        }

        private void Loaded(string data)
        {
            _isLoaded = true;
            _data = data;
        }
    }
}