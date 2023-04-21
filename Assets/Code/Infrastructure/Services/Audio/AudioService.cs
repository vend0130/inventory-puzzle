using Code.Infrastructure.Factories.Audio;

namespace Code.Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly IAudioFactory _audioFactory;

        public AudioService(IAudioFactory audioFactory)
        {
            _audioFactory = audioFactory;
        }
    }
}