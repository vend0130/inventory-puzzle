using System.Collections.Generic;
using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Game.InventorySystem
{
    public class InventoryGame : MonoBehaviour
    {
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private DragItems _dragItems;

        [field: SerializeField, Space] public LootInventory LootInventory;

        [field: SerializeField, Space] public Canvas CanvasWithItems { get; private set; }
        [field: SerializeField, ReadOnly] public List<BaseItem> Items { get; private set; }

        private Vector2Int _previousScreenSize;

        private void Awake()
        {
            _pointerHandler.LeftDownHandler += _dragItems.LeftDown;
            _pointerHandler.RightDownHandler += _dragItems.RightDown;
            _pointerHandler.DragHandler += _dragItems.Drag;
            _pointerHandler.UpHandler += _dragItems.Up;
            _pointerHandler.RightClickHandler += _dragItems.RightClick;
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
            _pointerHandler.LeftDownHandler -= _dragItems.LeftDown;
            _pointerHandler.RightDownHandler -= _dragItems.RightDown;
            _pointerHandler.DragHandler -= _dragItems.Drag;
            _pointerHandler.UpHandler -= _dragItems.Up;
            _pointerHandler.RightClickHandler -= _dragItems.RightClick;
        }

        public void CreateArrayItems() =>
            Items = new List<BaseItem>(CanvasWithItems.transform.childCount);
    }
}