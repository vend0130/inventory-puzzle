﻿using System;
using System.Threading;
using Code.Data;
using Code.Extensions;
using Code.Infrastructure.Services.Progress;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService, IDisposable
    {
        private const string ProgressKey = "Progress";

        private readonly IProgressService _progressService;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public SaveLoadService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public void Dispose()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }

        public void Save() =>
            PlayerPrefs.SetString(ProgressKey, _progressService.ProgressData.ToJson());

        public async UniTask<ProgressData> Load()
        {
            await UniTask.Yield(cancellationToken: _cancellation.Token);
            return PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<ProgressData>();
        }
    }
}