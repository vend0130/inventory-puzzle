using Code.Data;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save();
        UniTask<ProgressData> Load();
    }
}