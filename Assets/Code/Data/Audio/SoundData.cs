using System;
using UnityEngine;

namespace Code.Data.Audio
{
    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField] public SoundType Type { get; private set; }
        [field: SerializeField] public MixerGroup MixerGroup { get; private set; }
        [field: SerializeField] public float Volume { get; private set; } = 1;
        [field: SerializeField] public float Pitch { get; private set; } = 1;
    }
}