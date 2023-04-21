using UnityEngine;

namespace Code.Infrastructure.Factories.Audio
{
    public interface IAudioFactory
    {
        void CreateParent();
        AudioSource Create(string name);
    }
}