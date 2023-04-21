using Code.Data.Audio;

namespace Code.Infrastructure.Services.Audio
{
    public interface IAudioService
    {
        void Init();
        void Play(SoundType soundType);
    }
}