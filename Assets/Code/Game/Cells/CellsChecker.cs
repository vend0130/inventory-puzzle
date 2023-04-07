﻿using System.Collections.Generic;
using Code.Game.Inventory;
using UnityEngine;

namespace Code.Game.Cells
{
    public static class CellsChecker
    {
        public static bool TryTapOnCell(IInventory inventory, Vector2 position, out CellView cell)
        {
            for (int i = 0; i < inventory.Cells.Length; i++)
            {
                if (TapOnCell(inventory.Cells[i], position, out cell))
                    return true;
            }

            cell = null;
            return false;
        }

        public static bool TryEnterOnCell(IInventory inventory, List<Vector2> positions,
            out List<CellView> cells, out Vector2 cellPosition, out Vector2 itemCellPosition)
        {
            cells = new List<CellView>();

            cellPosition = Vector2.zero;
            itemCellPosition = Vector2.zero;

            for (int i = 0; i < inventory.Cells.Length; i++)
                EnterOnCell(inventory.Cells[i], positions, cells, ref cellPosition, ref itemCellPosition);

            return cells.Count > 0;
        }

        public static bool TryDropInNewCells(List<CellView> previousDragCells, int cellsCountForItem)
        {
            foreach (CellView cells in previousDragCells)
            {
                if (!cells.Free)
                    return false;
            }

            return previousDragCells.Count > 0 && previousDragCells.Count == cellsCountForItem;
        }

        private static bool TapOnCell(CellView currentCell, Vector2 position, out CellView cell)
        {
            cell = null;

            if (currentCell.Free)
                return false;

            if (Collision(position, currentCell))
            {
                cell = currentCell;
                return true;
            }

            return false;
        }

        private static void EnterOnCell(CellView currentCell, List<Vector2> positions, List<CellView> cells,
            ref Vector2 cellPosition, ref Vector2 itemCellPosition)
        {
            bool first = false;
            foreach (var position in positions)
            {
                if (!Collision(position, currentCell))
                    continue;

                cells.Add(currentCell);

                if (first)
                    continue;

                cellPosition = currentCell.CenterPoint;
                itemCellPosition = position;
                first = true;
            }
        }

        private static bool Collision(Vector2 position, CellView currentCell) =>
            position.x >= currentCell.StartPoint.x && position.x < currentCell.EndPoint.x &&
            position.y >= currentCell.StartPoint.y && position.y < currentCell.EndPoint.y;
    }
}