using System.Collections.Generic;
using Code.Data;
using Code.Data.Localize;
using Code.Extensions;
using Code.Game;
using Code.Game.Cells;
using Code.Game.InventorySystem;
using Code.Game.InventorySystem.Drag;
using Code.Game.Item.Items;
using Code.Game.ItemInfo;
using Code.Infrastructure.Services.Audio;
using Code.Infrastructure.Services.Progress;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IProgressService _progressService;
        private readonly IAudioService _audioService;
        private readonly LevelsData _levelsData;
        private readonly LocalizeConfig _localizeConfig;
        public InventoryGame InventoryGame { get; private set; }
        public SimplyMenu SimplyMenu { get; private set; }
        public GamePlayUI GamePlayUI { get; private set; }
        public ItemMenu ItemMenu { get; private set; }
        public DragItems DragItems { get; private set; }
        public PointerHandler PointerHandler { get; private set; }

        private readonly List<string> _backgroundsPaths;

        private ItemInfoView _itemInfo;
        private BlurChanger _background;

        public GameFactory(IProgressService progressService, IAudioService audioService,
            LevelsData levelsData, LocalizeConfig localizeConfig)
        {
            _progressService = progressService;
            _audioService = audioService;
            _levelsData = levelsData;
            _localizeConfig = localizeConfig;

            _backgroundsPaths = new List<string>()
            {
                AssetPath.Background1Path, AssetPath.Background2Path,
                AssetPath.Background3Path, AssetPath.Background4Path
            };
        }

        public void CreateBackground() =>
            _background = Instantiate(_backgroundsPaths.GetRandomElement()).GetComponent<BlurChanger>();

        public void CreateInfoPanel()
        {
            GameObject panel = Instantiate(AssetPath.InfoPanelsPath);

            ItemMenu = panel.GetComponent<ItemMenu>();
            ItemMenu.Init(_audioService, _localizeConfig.ItemMenu);

            ItemMenu.LockView.InitAudioService(_audioService);
            _itemInfo = ItemMenu.Info;
        }

        public void CreateGamePlayUI()
        {
            GamePlayUI = Instantiate(AssetPath.GamePlayUIPath).GetComponent<GamePlayUI>();
            GamePlayUI.ChangeText(_localizeConfig.Exit);
        }

        public void CreateLevel()
        {
            GameObject prefab = _levelsData.GetLevel(_progressService.ProgressData.CurrentLevel);
            var inventoryObject = Instantiate(prefab);

            InventoryGame = inventoryObject.GetComponent<InventoryGame>();
            InventoryGame.Init(_background, _localizeConfig.Loot, _localizeConfig.Inventory);

            SimplyMenu = inventoryObject.GetComponent<SimplyMenu>();
            SimplyMenu.SoundButton.ChangeState(_audioService.EffectsState);
            SimplyMenu.Init(_progressService.ProgressData.CurrentLevel + 1,
                _levelsData.CountLevels, _localizeConfig.Level);

            DragItems = InventoryGame.DragItems;
            DragItems.InitAudioService(_audioService);

            PointerHandler = InventoryGame.PointerHandler;

            foreach (BaseItem item in InventoryGame.Items)
                item.Init(ItemMenu, _itemInfo);
        }

        public BaseItem CreateItem(BaseItem parentItem, int index, Vector2 position)
        {
            GameObject prefab = parentItem.AdditionalDatas[index].Prefab;
            Transform parent = InventoryGame.CanvasWithItems.transform;
            BaseItem baseItem = Instantiate(parent, prefab, position).GetComponent<BaseItem>();

            baseItem.LoadItem(InventoryGame.CanvasWithItems.sortingOrder);
            baseItem.Init(ItemMenu, ItemMenu.Info);
            baseItem.ChangeInventory(parentItem.CurrentInventor);
            baseItem.AddParentItem(parentItem, parentItem.AdditionalDatas[index].Type);

            baseItem.ChangeDistance(baseItem.CurrentInventor.GetCurrentDistance());
            CellsHelper.ChangeOffsetItem(baseItem);
            baseItem.ChangeOffset();

            return baseItem;
        }

        private GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        private GameObject Instantiate(GameObject prefab) =>
            Object.Instantiate(prefab);

        private GameObject Instantiate(Transform parent, GameObject prefab, Vector2 at) =>
            Object.Instantiate(prefab, at, Quaternion.identity, parent);
    }
}