using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Item
{
    public static class ItemHelper
    {
        private const float SmallOffset = .015f;

        public static RotationType GetRotationType(float eulerAngles)
        {
            int angle = Mathf.Abs((int)Mathf.Round(eulerAngles));
            switch (angle)
            {
                case 0:
                    return RotationType.Top;
                case 90:
                    return RotationType.Left;
                case 180:
                    return RotationType.Bottom;
                case 270:
                    return RotationType.Right;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void CalculateBounds(RotationType rotationType, Vector2 position,
            float distance, Vector2Int size, out Vector2 startPoint, out Vector2 endPoint)
        {
            Vector2 halfSizeItem = new Vector2(distance * size.x / 2, distance * size.y / 2);

            switch (rotationType)
            {
                case RotationType.Top:
                    startPoint = new Vector2(position.x - halfSizeItem.x, position.y + halfSizeItem.y);
                    endPoint = new Vector2(position.x + halfSizeItem.x, position.y - halfSizeItem.y);
                    break;
                case RotationType.Bottom:
                    startPoint = new Vector2(position.x + halfSizeItem.x, position.y - halfSizeItem.y);
                    endPoint = new Vector2(position.x - halfSizeItem.x, position.y + halfSizeItem.y);
                    break;
                case RotationType.Left:
                    startPoint = new Vector2(position.x - halfSizeItem.y, position.y - halfSizeItem.x);
                    endPoint = new Vector2(position.x + halfSizeItem.y, position.y + halfSizeItem.x);
                    break;
                case RotationType.Right:
                    startPoint = new Vector2(position.x + halfSizeItem.y, position.y + halfSizeItem.x);
                    endPoint = new Vector2(position.x - halfSizeItem.y, position.y - halfSizeItem.x);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Vector2 GetStartPointCell(RotationType rotationType, Vector2 startPoint, float distance)
        {
            switch (rotationType)
            {
                case RotationType.Top:
                    return new Vector2(startPoint.x + distance / 2,
                        startPoint.y - distance / 2);
                case RotationType.Bottom:
                    return new Vector2(startPoint.x - distance / 2,
                        startPoint.y + distance / 2);
                case RotationType.Left:
                    return new Vector2(startPoint.x + distance / 2,
                        startPoint.y + distance / 2);
                case RotationType.Right:
                    return new Vector2(startPoint.x - distance / 2,
                        startPoint.y - distance / 2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static List<(Vector2, CellInItemData)> GetCellsPositions(RotationType rotationType, WidthData[] grid,
            float distance, Vector2 startPointCell)
        {
            List<(Vector2, CellInItemData)> positions = new List<(Vector2, CellInItemData)>(grid.Length * grid[0].Width.Length);
            Vector2Int multiply = GetMultiply(rotationType);

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Width.Length; x++)
                {
                    Vector2 position = GetCellPosition(startPointCell, new Vector2Int(x, y),
                        rotationType, distance, multiply);

                    positions.Add((position, grid[y].Width[x]));
                }
            }

            return positions;
        }

        public static List<(Vector2, CellInItemData)> GetCellsPositionsWithType(RotationType rotationType, WidthData[] grid,
            float distance, Vector2 startPointCell)
        {
            List<(Vector2, CellInItemData)> positions = new List<(Vector2, CellInItemData)>(grid.Length * grid[0].Width.Length);
            Vector2Int multiply = GetMultiply(rotationType);

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Width.Length; x++)
                {
                    Vector2 position = GetCellPosition(startPointCell, new Vector2Int(x, y),
                        rotationType, distance, multiply);

                    positions.Add((position, grid[y].Width[x]));
                }
            }

            return positions;
        }

        private static Vector2 GetCellPosition(Vector2 startPointCell, Vector2Int indexes,
            RotationType rotationType, float distance, Vector2Int multiply)
        {
            Vector2 position = startPointCell;

            int tempX = indexes.x;
            int tempY = indexes.y;
            Calculate(rotationType, ref tempX, ref tempY);

            position.x += (distance * tempX + SmallOffset) * multiply.x;
            position.y -= (distance * tempY + SmallOffset) * multiply.y;

            return position;
        }

        private static Vector2Int GetMultiply(RotationType rotationType)
        {
            switch (rotationType)
            {
                case RotationType.Top:
                    return new Vector2Int(1, 1);
                case RotationType.Bottom:
                    return new Vector2Int(-1, -1);
                case RotationType.Left:
                    return new Vector2Int(1, -1);
                case RotationType.Right:
                    return new Vector2Int(-1, 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Calculate(RotationType rotationType, ref int x, ref int y)
        {
            switch (rotationType)
            {
                case RotationType.Top:
                case RotationType.Bottom:
                    break;
                case RotationType.Right:
                case RotationType.Left:
                    (x, y) = (y, x);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}