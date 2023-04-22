using System.Collections.Generic;
using Code.Game.Item;
using Code.Game.Item.Items;

namespace Code.Extensions
{
    public static class ItemCellDataExtension
    {
        /// <summary>
        /// Добавляем в список клеток объект
        /// </summary>
        public static void AddItemInCell(this List<ItemCellData> cells, BaseItem item) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.AddItem(item);
            });

        /// <summary>
        /// Удаляем из списка клеток объект
        /// </summary>
        public static void RemoveItemInCell(this List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.RemoveItem();
            });

        /// <summary>
        /// Включаем подсвечивание клетки, означающее, что мы  можем объеденить объекты (например можем дропнуть магазин в автомат)
        /// </summary>
        public static void CellsCombine(this List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.CombineEnter();
            });

        /// <summary>
        /// Включаем подсвечивание клетки в которых находится объект
        /// </summary>
        public static void CellsDrop(this List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.Drop();
            });

        /// <summary>
        /// Включаем подсвечивание клетки, означающее, что мы  можем туда дропнуть объект
        /// </summary>
        public static void CellsEnter(this List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.Enter();
            });

        /// <summary>
        /// Включаем подсвечивание клетки, означающее, что мы не можем туда дропнуть объект
        /// </summary>
        public static void CellsBadEnter(this List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.BadEnter();
            });

        /// <summary>
        /// Отключаем подсвечивание клетки
        /// </summary>
        public static void CellsExit(this List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.Exit();
            });
    }
}