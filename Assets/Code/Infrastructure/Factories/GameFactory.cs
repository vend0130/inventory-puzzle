using System.Collections.Generic;
using Code.Extensions;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        public GamePlayUI GamePlayUI { get; private set; }

        private readonly List<string> _backgroundsPaths;

        public GameFactory()
        {
            _backgroundsPaths = new List<string>()
            {
                AssetPath.Background1Path, AssetPath.Background2Path
            };
        }

        public void CreateBackground()
        {
            Instantiate(_backgroundsPaths.GetRandomElement()).GetComponent<GamePlayUI>();
        }

        public void CreateGamePlayUI()
        {
            GamePlayUI = Instantiate(AssetPath.GamePlayUIPath).GetComponent<GamePlayUI>();
        }

        private GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
    }
}