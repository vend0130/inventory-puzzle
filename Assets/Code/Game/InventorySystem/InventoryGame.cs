using System.Collections.Generic;
using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item.Items;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Game.InventorySystem
{
    public class InventoryGame : MonoBehaviour
    {
        [field: SerializeField] public PointerHandler PointerHandler { get; private set; }
        [field: SerializeField] public DragItems DragItems { get; private set; }

        [field: SerializeField, Space] public LootInventory LootInventory;

        [field: SerializeField, Space] public Canvas CanvasWithItems { get; private set; }
        [field: SerializeField, ReadOnly] public List<BaseItem> Items { get; private set; }

        private Vector2Int _previousScreenSize;

        private void Awake()
        {
            PointerHandler.LeftDownHandler += DragItems.LeftDown;
            PointerHandler.RightDownHandler += DragItems.RightDown;
            PointerHandler.DragHandler += DragItems.Drag;
            PointerHandler.UpHandler += DragItems.Up;
            PointerHandler.RightClickHandler += DragItems.RightClick;

            DragItems.DropNewItemHandler += AddItem;
        }

        private void Start()
        {
            LootInventory.UpdateGrid(Items);
            _previousScreenSize = CellsHelper.CurrentSizeScreen();
        }

        private void Update()
        {
            if (CellsHelper.CurrentSizeScreen() != _previousScreenSize)
                LootInventory.UpdateGrid(Items);

            _previousScreenSize = CellsHelper.CurrentSizeScreen();
        }

        private void OnDestroy()
        {
            PointerHandler.LeftDownHandler -= DragItems.LeftDown;
            PointerHandler.RightDownHandler -= DragItems.RightDown;
            PointerHandler.DragHandler -= DragItems.Drag;
            PointerHandler.UpHandler -= DragItems.Up;
            PointerHandler.RightClickHandler -= DragItems.RightClick;

            DragItems.DropNewItemHandler -= AddItem;
        }

        public void CreateArrayItems() =>
            Items = new List<BaseItem>(CanvasWithItems.transform.childCount);

        private void AddItem(BaseItem item) =>
            Items.Add(item);
    }
}