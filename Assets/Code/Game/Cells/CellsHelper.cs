using System;
using System.Collections.Generic;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item;
using Code.Game.Item.Items;
using UnityEditor;
using UnityEngine;

namespace Code.Game.Cells
{
    public static class CellsHelper
    {
        private const int DefaultWidthScreen = 1920;
        private const int DefaultHeightScreen = 1080;

        //note: высчитываем дистанцию с учетом размера экрана
        public static float GetCurrentDistance(float distance)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && DefaultWidthScreen != CurrentSizeScreen().x)
                Debug.LogError("In Editor Mode use screen size 1920x1080");
#endif

            Vector2 scaler = GetScaler();

            float currentScaler = scaler.x < scaler.y ? scaler.x : scaler.y;

            return currentScaler * distance;
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

        //note: проходим по всем ячейкм и если позиция тапа в ячейки совподают возвращяем true
        public static bool TryTapOnCell(BaseInventory inventory, Vector2 position, out CellView cell)
        {
            for (int i = 0; i < inventory.Cells.Length; i++)
            {
                if (TapOnCell(inventory.Cells[i], position, out cell))
                    return true;
            }

            cell = null;
            return false;
        }

        //note: получаем все клетки в инвентаре, над которыми есть объект (который мы драгаем)
        public static bool TryEnterOnCell(BaseInventory inventory, BaseItem item, out List<ItemCellData> cells)
        {
            //получаем позиции асех ячеек в инвентаре, даже скрытые (например магазин отключен)
            List<(Vector2, CellInItemData)> positions = item.GetCellsPositions();
            cells = new List<ItemCellData>();

            for (int i = 0; i < inventory.Cells.Length; i++)
                EnterOnCell(inventory.Cells[i], item, positions, cells);

            return cells.Count > 0;
        }

        public static void ChangeOffsetItem(BaseItem item)
        {
            List<(Vector2, CellInItemData)> positions = item.GetCellsPositions();

            foreach (var position in positions)
            {
                foreach (var cell in item.ParentCells)
                {
                    if (position.Item2.Activate && Collision(position.Item1, cell.CellOnGrid))
                    {
                        item.ChangeLastOffset(cell.CellOnGrid.CenterPoint - position.Item1);
                        return;
                    }
                }
            }
        }

        //note; считаем количество ячеек для объекта (убирая неактивные)
        public static int DropCellCount(List<ItemCellData> cells, int count)
        {
            if (cells == null || cells.Count == 0)
            {
                if (count == 0)
                    throw new Exception("not cell in item");

                return count;
            }

            int counter = 0;
            foreach (var cell in cells)
            {
                if (cell.CellInItem.Activate)
                    counter++;
            }

            return counter;
        }

        private static Vector2 GetScaler()
        {
            Vector2Int currentSize = CurrentSizeScreen();

            float scalerX = (float)currentSize.x / DefaultWidthScreen;
            float scalerY = (float)currentSize.y / DefaultHeightScreen;

            return new Vector2(scalerX, scalerY);
        }

        //note: попали ли по ячейке
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

        //note: проходим по позициям клеток в инвентаре
        private static void EnterOnCell(CellView currentCell, BaseItem item,
            List<(Vector2, CellInItemData)> positions, List<ItemCellData> cells)
        {
            bool first = false;
            foreach (var position in positions)
            {
                if (currentCell.Lock || !Collision(position.Item1, currentCell))
                    continue;

                cells.Add(new ItemCellData(currentCell, position.Item2));

                if (first || !position.Item2.Activate)
                    continue;

                item.ChangeLastOffset(currentCell.CenterPoint - position.Item1);
                first = true;
            }
        }

        public static bool Collision(Vector2 position, CellView currentCell) =>
            position.x >= currentCell.StartPoint.x && position.x < currentCell.EndPoint.x &&
            position.y >= currentCell.StartPoint.y && position.y < currentCell.EndPoint.y;
    }
}