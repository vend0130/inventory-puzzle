using System;
using UnityEngine;

namespace Code.Data.Audio
{
    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public AudioClip Audio { get; private set; }
        [field: SerializeField] public SoundType Type { get; private set; }
    }
}