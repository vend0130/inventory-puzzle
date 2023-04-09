using System.Collections.Generic;
using Code.Game.Cells;
using Code.Game.Item;
using Code.Utils.Readonly;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Inventory
{
    public class LootInventory : MonoBehaviour, IInventory
    {
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private DragItems _dragItems;
        [field: SerializeField] public Canvas CanvasWithItems { get; private set; }
        [field: SerializeField] public GridLayoutGroup LayoutGroup { get; private set; }
        [field: SerializeField] public RectTransform ParentForCells { get; private set; }
        [field: SerializeField, ReadOnly] public CellView[] Cells { get; private set; }
        [field: SerializeField, ReadOnly] public List<ItemView> Items { get; private set; }

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
            UpdateGrid();
            _previousScreenSize = CellsHelper.CurrentSizeScreen();
        }

        //TODO: to main class
        private void Update()
        {
            if (CellsHelper.CurrentSizeScreen() != _previousScreenSize)
                UpdateGrid();

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

        public void CreateArray()
        {
            Cells = new CellView[ParentForCells.childCount];
            Items = new List<ItemView>(CanvasWithItems.transform.childCount);
        }

        public void UpdateGrid()
        {
            RefreshGrid();

            float distance = GetCurrentDistance(LayoutGroup.cellSize.x + LayoutGroup.spacing.x);

            for (int i = 0; i < Cells.Length; i++)
                Cells[i].RecalculatePoints(distance);

            foreach (var item in Items)
            {
                item.ChangeDistance(distance);

                CellsHelper.ChangeOffsetItem(item);

                item.ChangeOffset();
                item.transform.position = item.GetTargetPosition();
            }
        }

        public void RefreshGrid()
        {
            LayoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
            LayoutGroup.enabled = false;
        }

        public float GetCurrentDistance(float distance) =>
            CellsHelper.GetCurrentDistance(distance);
    }
}