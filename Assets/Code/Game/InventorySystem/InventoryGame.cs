using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.InventorySystem.Drag;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item.Items;
using Code.UI;
using Code.Utils.Readonly;
using TMPro;
using UnityEngine;

namespace Code.Game.InventorySystem
{
    public class InventoryGame : MonoBehaviour
    {
        [field: SerializeField] public PointerHandler PointerHandler { get; private set; }
        [field: SerializeField] public DragItems DragItems { get; private set; }

        [field: SerializeField, Space] public LootInventory LootInventory { get; private set; }
        [field: SerializeField, Space] public MainInventory MainInventory { get; private set; }

        [field: SerializeField, Space] public Canvas CanvasWithItems { get; private set; }
        [field: SerializeField, ReadOnly] public List<BaseItem> Items { get; private set; }

        [SerializeField, Space] private TextMeshProUGUI _levelText;
        [field: SerializeField] public MenuButton AgainButton { get; private set; }
        [field: SerializeField] public MenuButton SoundButton { get; private set; }

        public event Action AllItemsInInventoryHandler;

        private Vector2Int _previousScreenSize;

        private void Awake()
        {
            PointerHandler.LeftDownHandler += DragItems.LeftDown;
            PointerHandler.RightDownHandler += DragItems.RightDown;
            PointerHandler.DragHandler += DragItems.Drag;
            PointerHandler.UpHandler += DragItems.Up;
            PointerHandler.RightClickHandler += DragItems.RightClick;

            DragItems.DestroyItemHandler += RemoveItem;
            DragItems.ItemEndMoveHandler += ItemEndMove;
        }

        private void Start()
        {
            foreach (var item in Items)
                item.ParentCells.CellsDrop();

            ChangeInventory();
            _previousScreenSize = CellsHelper.CurrentSizeScreen();
        }

#if UNITY_EDITOR || UNITY_WEBGL
        private void Update()
        {
            if (CellsHelper.CurrentSizeScreen() != _previousScreenSize)
                ChangeInventory();

            _previousScreenSize = CellsHelper.CurrentSizeScreen();
        }
#endif

        private void OnDestroy()
        {
            PointerHandler.LeftDownHandler -= DragItems.LeftDown;
            PointerHandler.RightDownHandler -= DragItems.RightDown;
            PointerHandler.DragHandler -= DragItems.Drag;
            PointerHandler.UpHandler -= DragItems.Up;
            PointerHandler.RightClickHandler -= DragItems.RightClick;

            DragItems.DestroyItemHandler -= RemoveItem;
            DragItems.ItemEndMoveHandler -= ItemEndMove;
        }

        public void Init(int currentLevel, int maxLevel)
        {
            _levelText.text = $"{Constants.LevelTextPrefix} {currentLevel} ";
            _levelText.text += string.Format(Constants.LevelTextPostfix, maxLevel);
        }

        public void CreateArrayItems() =>
            Items = new List<BaseItem>(CanvasWithItems.transform.childCount);

        public void AddItem(BaseItem item) =>
            Items.Add(item);

        private void ChangeInventory()
        {
            float distance = LootInventory.GetCurrentDistance();

            LootInventory.UpdateGrid(distance);
            MainInventory.UpdateGrid(distance);

            foreach (var item in Items)
            {
                item.ChangeDistance(distance);

                CellsHelper.ChangeOffsetItem(item);

                item.ChangeOffset();
                item.transform.position = item.GetTargetPosition();
            }
        }

        private void RemoveItem(BaseItem item) =>
            Items.Remove(item);

        private void ItemEndMove()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].CurrentInventor == LootInventory)
                    return;
            }

            DragItems.enabled = false;
            PointerHandler.enabled = false;
            AllItemsInInventoryHandler?.Invoke();
        }
    }
}