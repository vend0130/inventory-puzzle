using Code.Data;

namespace Code.Infrastructure.Services.Progress
{
    public interface IProgressService
    {
        ProgressData ProgressData { get; }
        void Load();
        void NextLevel();
    }
}