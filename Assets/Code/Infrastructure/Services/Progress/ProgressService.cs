using Code.Data;

namespace Code.Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        public ProgressData ProgressData { get; private set; }

        private readonly LevelsData _levelsData;

        public ProgressService(LevelsData levelsData) =>
            _levelsData = levelsData;

        public void ChangeProgressData(ProgressData progressData) =>
            ProgressData = progressData;

        public void NextLevel()
        {
            ProgressData.CurrentLevel++;

            if (ProgressData.CurrentLevel >= _levelsData.CountLevels)
                ProgressData.CurrentLevel = 0;
        }
    }
}