using Code.Game.Cells;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Game.Inventory
{
    public class LootInventory : MonoBehaviour, IInventory
    {
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private DragItems _dragItems;
        [field: SerializeField] public Canvas CanvasWithItems { get; private set; }
        [field: SerializeField] public RectTransform ParentForCells { get; private set; }
        [field: SerializeField, ReadOnly] public CellView[] Cells { get; private set; }

        private void Awake()
        {
            _pointerHandler.DownHandler += _dragItems.Down;
            _pointerHandler.DragHandler += _dragItems.Drag;
            _pointerHandler.UpHandler += _dragItems.Up;
        }

        private void OnDestroy()
        {
            _pointerHandler.DownHandler -= _dragItems.Down;
            _pointerHandler.DragHandler -= _dragItems.Drag;
            _pointerHandler.UpHandler -= _dragItems.Up;
        }


        public void CreateCells()
        {
            Cells = new CellView[ParentForCells.childCount];
        }
    }
}