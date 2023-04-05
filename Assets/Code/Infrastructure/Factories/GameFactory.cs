using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        public GamePlayUI GamePlayUI { get; private set; }

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