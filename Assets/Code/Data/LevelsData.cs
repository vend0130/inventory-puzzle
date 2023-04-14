using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(LevelsData), menuName = "Static Data/" + nameof(LevelsData))]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] private GameObject[] Levels;

        public int CountLevels => Levels.Length;

        public GameObject GetLevel(int level) =>
            Levels[level];
    }
}