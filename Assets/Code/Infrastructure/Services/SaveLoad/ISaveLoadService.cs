using Code.Data;

namespace Code.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save();
        ProgressData Load();
    }
}