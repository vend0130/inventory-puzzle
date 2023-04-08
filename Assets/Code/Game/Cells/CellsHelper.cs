using System.Collections.Generic;
using Code.Game.Inventory;
using Code.Game.Item;
using UnityEditor;
using UnityEngine;

namespace Code.Game.Cells
{
    public static class CellsHelper
    {
        private const int DefaultWidthScreen = 1920;
        private const int DefaultHeightScreen = 1080;

        public static float GetCurrentDistance(float distance)
        {
            Vector2Int currentSize = CurrentSizeScreen();

#if UNITY_EDITOR
            if (!Application.isPlaying && DefaultWidthScreen != currentSize.x)
                Debug.LogError("In Editor Mode use screen size 1920x1080");
#endif

            float scalerX = (float)currentSize.x / DefaultWidthScreen;
            float scalerY = (float)currentSize.y / DefaultHeightScreen;

            float scaler = scalerX < scalerY ? scalerX : scalerY;

            return scaler * distance;
        }

        public static Vector2Int CurrentSizeScreen()
        {
#if UNITY_EDITOR
            Vector2 size = Handles.GetMainGameViewSize();
            return new Vector2Int((int)size.x, (int)size.y);
#else
            return new Vector2Int(Screen.width, Screen.height);
#endif
        }

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

        public static bool TryEnterOnCell(IInventory inventory, ItemView item, out List<CellView> cells)
        {
            List<Vector2> positions = item.GetCellsPositions();
            cells = new List<CellView>();

            for (int i = 0; i < inventory.Cells.Length; i++)
                EnterOnCell(inventory.Cells[i], item, positions, cells);

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

        private static void EnterOnCell(CellView currentCell, ItemView item,
            List<Vector2> positions, List<CellView> cells)
        {
            bool first = false;
            foreach (var position in positions)
            {
                if (!Collision(position, currentCell))
                    continue;

                cells.Add(currentCell);

                if (first)
                    continue;

                item.ChangeLastOffset(currentCell.CenterPoint - position);

                first = true;
            }
        }

        private static bool Collision(Vector2 position, CellView currentCell) =>
            position.x >= currentCell.StartPoint.x && position.x < currentCell.EndPoint.x &&
            position.y >= currentCell.StartPoint.y && position.y < currentCell.EndPoint.y;
    }
}