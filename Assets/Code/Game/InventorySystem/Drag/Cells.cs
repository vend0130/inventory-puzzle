using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item;
using Code.Game.Item.Items;
using UnityEngine;

namespace Code.Game.InventorySystem.Drag
{
    public class Cells : MonoBehaviour
    {
        [SerializeField] private BaseInventory[] _inventories;

        public List<ItemCellData> PreviousDragCells { get; private set; } = new List<ItemCellData>();

        public void ChangeCells(List<ItemCellData> cells) =>
            PreviousDragCells = cells;

        public bool TryTapOnCell(BaseInventory inventory, Vector2 position, out CellView cell) =>
            CellsHelper.TryTapOnCell(inventory, position, out cell);

        public bool TryEnterOnCell(BaseItem item, out List<ItemCellData> cells) =>
            CellsHelper.TryEnterOnCell(item.CurrentInventor, item, out cells);

        public void ChangeCellsWhenDragItem(BaseItem item)
        {
            if (!TryGetInventory(item.transform.position, out BaseInventory inventory) ||
                !CellsHelper.TryEnterOnCell(inventory, item, out List<ItemCellData> cells))
            {
                ClearPreviousDragCells();
                return;
            }

            item.ChangeOffset();

            PreviousDragCells.CellsExit();
            PreviousDragCells = cells;

            EnterCellsWhenDragItem(item);
        }

        private void EnterCellsWhenDragItem(BaseItem item)
        {
            switch (GetDropType(item, out BaseItem _))
            {
                case DropType.Combine:
                    PreviousDragCells.CellsCombine();
                    break;
                case DropType.Drop:
                    PreviousDragCells.CellsEnter();
                    break;
                case DropType.FailDrop:
                    PreviousDragCells.CellsBadEnter();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DropType GetDropType(BaseItem item, out BaseItem combineItem)
        {
            combineItem = null;

            int countCells = CellsHelper.DropCellCount(PreviousDragCells);
            int countCellInItem = CellsHelper.DropCellCount(item.ParentCells, item.CellsCountForItem);

            if (countCells != countCellInItem)
                return DropType.FailDrop;

            bool isFailDrop = false;

            foreach (var cell in PreviousDragCells)
            {
                if (!cell.CellInItem.Activate)
                    continue;

                if (TryCombine(cell, item.ItemType, out combineItem))
                    return DropType.Combine;

                if (!cell.CellOnGrid.Free)
                    isFailDrop = true;
            }

            return isFailDrop ? DropType.FailDrop : DropType.Drop;
        }

        public void CellsEnter() =>
            PreviousDragCells.CellsEnter();

        public void CellsExit() =>
            PreviousDragCells.CellsExit();

        public bool TryGetInventory(Vector2 position, out BaseInventory inventory) =>
            InventoryHelper.TryGetInventory(position, _inventories, out inventory);

        public void ClearPreviousDragCells()
        {
            PreviousDragCells.CellsExit();
            PreviousDragCells.Clear();
        }

        private bool TryCombine(ItemCellData cell, ItemType dragItemType, out BaseItem combineItem)
        {
            combineItem = null;

            if (cell.CellOnGrid.Free ||
                !cell.CellOnGrid.Item.TryGetCountCellsForAdditional(dragItemType, out int count))
                return false;

            if (cell.CellOnGrid.Item.AdditionalIsActivate(dragItemType))
                return false;

            List<ItemCellData> cellAnotherItem = cell.CellOnGrid.Item.ParentCells;

            int countCombineCells = 0;

            foreach (var anotherCell in cellAnotherItem)
            {
                // Debug.Log(anotherCell.CellOnGrid.Free + "  " + anotherCell.CellInItem.Type);
                if (anotherCell.CellOnGrid.Free && anotherCell.CellInItem.Type == dragItemType)
                    countCombineCells++;
            }

            combineItem = cell.CellOnGrid.Item;

            return countCombineCells == count;
        }
    }
}