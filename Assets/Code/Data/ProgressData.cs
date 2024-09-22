using System;
using UnityEngine;

namespace Code.Data
{
    [Serializable]
    public class ProgressData
    {
        [SerializeField] private int currentLevel;
        [SerializeField] private int openedLevel;
        [SerializeField] private bool sound;

        public int CurrentLevel => currentLevel;
        public int OpenedLevel => openedLevel;
        public bool Sound => sound;

        public void SetCurrentLevel(int value) =>
            currentLevel = value;

        public void SetOpenedLevel(int value) =>
            openedLevel = value;

        public void ChangeSoundState(bool value) =>
            sound = value;
    }
}