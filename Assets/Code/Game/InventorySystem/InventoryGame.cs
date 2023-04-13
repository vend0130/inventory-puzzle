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

        [field: SerializeField, Space] public LootInventory LootInventory { get; private set; }
        [field: SerializeField, Space] public MainInventory MainInventory { get; private set; }

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
            DragItems.DestroyItemHandler += RemoveItem;
        }

        private void Start()
        {
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

            DragItems.DropNewItemHandler -= AddItem;
            DragItems.DestroyItemHandler -= RemoveItem;
        }

        public void CreateArrayItems() =>
            Items = new List<BaseItem>(CanvasWithItems.transform.childCount);

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

        private void AddItem(BaseItem item) =>
            Items.Add(item);

        private void RemoveItem(BaseItem item) =>
            Items.Remove(item);
    }
}