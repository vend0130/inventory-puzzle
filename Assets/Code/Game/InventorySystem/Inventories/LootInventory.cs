using System.Collections.Generic;
using Code.Game.Cells;
using Code.Game.Item.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.InventorySystem.Inventories
{
    public class LootInventory : BaseInventory
    {
        [field: SerializeField] public GridLayoutGroup LayoutGroup { get; private set; }

        public void UpdateGrid(List<BaseItem> items)
        {
            RefreshGrid();

            float distance = GetCurrentDistance(LayoutGroup.cellSize.x + LayoutGroup.spacing.x);

            for (int i = 0; i < Cells.Length; i++)
                Cells[i].RecalculatePoints(distance);

            foreach (var item in items)
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