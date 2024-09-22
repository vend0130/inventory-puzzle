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

        public void ChangeLevel(int level) =>
            SetLevel(level);

        public void NextLevel()
        {
            SetLevel(ProgressData.CurrentLevel + 1);

            if (ProgressData.OpenedLevel < ProgressData.CurrentLevel)
                ProgressData.SetOpenedLevel(ProgressData.CurrentLevel);
        }

        private void SetLevel(int level)
        {
            ProgressData.SetCurrentLevel(level);

            if (ProgressData.CurrentLevel >= _levelsData.CountLevels || ProgressData.CurrentLevel < 0)
                ProgressData.SetCurrentLevel(0);
        }
    }
}