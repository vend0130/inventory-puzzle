using Code.Game.Cells;
using Code.Utils.Readonly;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.InventorySystem.Inventories
{
    public class BaseInventory : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public CellView[] Cells { get; private set; }
        [field: SerializeField, ReadOnly] public Rect Rect { get; private set; }
        [field: SerializeField] public RectTransform ParentForCells { get; private set; }

        [SerializeField] private GridLayoutGroup _layoutGroup;
        [SerializeField] private RectTransform _background;

        public void UpdateGrid(float distance)
        {
            RefreshGrid();

            for (int i = 0; i < Cells.Length; i++)
                Cells[i].RecalculatePoints(distance);

            ChangeRect();
        }

        public void ChangeRect()
        {
            Vector2 position = _background.transform.position;
            Vector2 rectMax = _background.rect.max;
            Rect = new Rect(position - rectMax, rectMax * 2);
        }

        public void RefreshGrid()
        {
            _layoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
            _layoutGroup.enabled = false;
        }

        public void CreateArray() =>
            Cells = new CellView[ParentForCells.childCount];

        public float GetCurrentDistance() =>
            CellsHelper.GetCurrentDistance(_layoutGroup.cellSize.x + _layoutGroup.spacing.x);
    }
}