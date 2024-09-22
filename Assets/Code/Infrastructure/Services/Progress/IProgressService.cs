using Code.Data;

namespace Code.Infrastructure.Services.Progress
{
    public interface IProgressService
    {
        ProgressData ProgressData { get; }
        void NextLevel();
        void ChangeProgressData(ProgressData progressData);
        void ChangeLevel(int level);
    }
}