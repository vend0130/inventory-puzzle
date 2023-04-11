using System.Collections.Generic;
using Code.Extensions;
using Code.Game.InventorySystem;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item;
using Code.Game.Item.Items;
using Code.Game.ItemInfo;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        public GamePlayUI GamePlayUI { get; private set; }
        public ItemMenu ItemMenu { get; private set; }
        public DragItems DragItems { get; private set; }
        public PointerHandler PointerHandler { get; private set; }

        private readonly List<string> _backgroundsPaths;

        private ItemInfoView _itemInfo;
        private InventoryGame _inventoryGame;

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
            _itemInfo = ItemMenu.Info;
        }

        public void CreateGamePlayUI() =>
            GamePlayUI = Instantiate(AssetPath.GamePlayUIPath).GetComponent<GamePlayUI>();

        public void CreateLevel()
        {
            _inventoryGame = Instantiate(AssetPath.LevelPath).GetComponent<InventoryGame>();
            DragItems = _inventoryGame.DragItems;
            PointerHandler = _inventoryGame.PointerHandler;

            foreach (BaseItem item in _inventoryGame.Items)
                item.Init(ItemMenu, _itemInfo);
        }

        public BaseItem CreateItem(ItemType itemType, Vector2 position, BaseInventory inventory)
        {
            Transform parent = _inventoryGame.CanvasWithItems.transform;
            BaseItem baseItem = Instantiate(parent, AssetPath.MagazinePath, position).GetComponent<BaseItem>();
            baseItem.LoadItem(_inventoryGame.CanvasWithItems.sortingOrder);
            baseItem.Init(ItemMenu, ItemMenu.Info);
            baseItem.ChangeInventory(inventory);
            return baseItem;
        }

        private GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        private GameObject Instantiate(Transform parent, string path, Vector2 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity, parent);
        }
    }
}