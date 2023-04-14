using Code.Data;

namespace Code.Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        public ProgressData ProgressData { get; private set; }

        private readonly LevelsData _levelsData;

        public ProgressService(ProgressData progressData, LevelsData levelsData)
        {
            ProgressData = progressData;
            _levelsData = levelsData;
        }

        public void Load() =>
            ProgressData.CurrentLevel = 0;

        public void NextLevel()
        {
            ProgressData.CurrentLevel++;

            if (ProgressData.CurrentLevel >= _levelsData.CountLevels)
                ProgressData.CurrentLevel = 0;
        }
    }
}