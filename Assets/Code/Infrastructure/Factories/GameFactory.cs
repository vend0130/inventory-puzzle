using System.Collections.Generic;
using Code.Extensions;
using Code.Game.InventorySystem;
using Code.Game.Item;
using Code.Game.ItemInfo;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        public GamePlayUI GamePlayUI { get; private set; }
        public ItemMenu ItemMenu { get; private set; }
        public ItemInfoView ItemInfo { get; private set; }

        private readonly List<string> _backgroundsPaths;

        public GameFactory()
        {
            _backgroundsPaths = new List<string>()
            {
                AssetPath.Background1Path, AssetPath.Background2Path
            };
        }

        public void CreateBackground() =>
            Instantiate(_backgroundsPaths.GetRandomElement());

        public void CreateInfoPanel()
        {
            GameObject panel = Instantiate(AssetPath.InfoPanelsPath);
            ItemMenu = panel.GetComponent<ItemMenu>();
            ItemInfo = ItemMenu.Info;
        }

        public void CreateGamePlayUI() =>
            GamePlayUI = Instantiate(AssetPath.GamePlayUIPath).GetComponent<GamePlayUI>();

        public void CreateLevel()
        {
            InventoryGame inventory = Instantiate(AssetPath.LevelPath).GetComponent<InventoryGame>();

            foreach (BaseItem item in inventory.Items)
                item.Init(ItemMenu, ItemInfo);
        }

        private GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
    }
}