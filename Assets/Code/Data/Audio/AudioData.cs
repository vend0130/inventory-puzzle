using UnityEngine;
using UnityEngine.Audio;

namespace Code.Data.Audio
{
    [CreateAssetMenu(fileName = nameof(AudioData), menuName = "Static Data/" + nameof(AudioData))]
    public class AudioData : ScriptableObject
    {
        [field: SerializeField] public AudioMixer AudioMixer { get; private set; }
        [field: SerializeField, Space] public SoundData[] SoundsDatas { get; private set; }
    }
}