using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Infrastructure.Factories.Audio
{
    public class AudioSourceFactory : IAudioFactory
    {
        private const string ParentName = " ParentForAudioSources";
        private const string AudioSourceName = " AudioSources";

        private Transform _parent;

        public void CreateParent()
        {
            _parent = new GameObject(ParentName).transform;
            Object.DontDestroyOnLoad(_parent);
        }

        public AudioSource Create(string name)
        {
            if (_parent == null)
                throw new Exception("parent for audio not created");

            var prefab = new GameObject($"{AudioSourceName}_{name}");

            prefab.transform.SetParent(_parent);

            var audioSource = prefab.AddComponent<AudioSource>();

            return audioSource;
        }
    }
}