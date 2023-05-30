using Code.Data;
using Code.Extensions;
using Code.Infrastructure.Services.Progress;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IProgressService _progressService;

        public SaveLoadService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public void Save() =>
            PlayerPrefs.SetString(ProgressKey, _progressService.ProgressData.ToJson());

        public ProgressData Load() =>
            PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<ProgressData>();
    }
}