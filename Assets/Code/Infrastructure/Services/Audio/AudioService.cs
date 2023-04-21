using System;
using System.Collections.Generic;
using Code.Data.Audio;
using Code.Infrastructure.Factories.Audio;
using UnityEngine;

namespace Code.Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly IAudioFactory _audioFactory;
        private readonly AudioData _data;
        private readonly Dictionary<SoundType, AudioSource> _audioCreated = new Dictionary<SoundType, AudioSource>();

        public AudioService(IAudioFactory audioFactory, AudioData data)
        {
            _audioFactory = audioFactory;
            _data = data;
        }

        public void Init() =>
            _audioFactory.CreateParent();

        public void Play(SoundType soundType)
        {
            if (!_audioCreated.ContainsKey(soundType))
                _audioCreated.Add(soundType, Configure(soundType));

            _audioCreated[soundType].Play();
        }

        private AudioSource Configure(SoundType soundType)
        {
            AudioSource audioSource = _audioFactory.Create(soundType.ToString());

            var data = GetSoundData(soundType);
            audioSource.clip = data.Clip;
            audioSource.volume = data.Volume;
            audioSource.pitch = data.Pitch;
            audioSource.outputAudioMixerGroup =
                _data.AudioMixer.FindMatchingGroups(data.MixerGroup.ToString())[0];

            return audioSource;
        }

        private SoundData GetSoundData(SoundType soundType)
        {
            foreach (SoundData data in _data.SoundsDatas)
            {
                if (data.Type == soundType)
                    return data;
            }

            throw new Exception($"not found {soundType} in AudioData");
        }
    }
}