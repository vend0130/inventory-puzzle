using Code.Data.Audio;

namespace Code.Infrastructure.Services.Audio
{
    public interface IAudioService
    {
        void Init(bool value);
        void Play(SoundType soundType);
        bool EffectsState { get; }
        void ChangeEffectState();
    }
}