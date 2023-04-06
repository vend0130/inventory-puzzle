using System;
using System.Collections.Generic;
using Code.Game.Cells;
using UnityEngine;

namespace Code.Game.Item
{
    [Serializable]
    public class DataA
    {
        public List<bool> A;
    }
    
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _distanceBetweenCells = 69f;
        [SerializeField] private int _width = 1;
        [SerializeField] private int _height = 1;
        [SerializeField] private bool _debug;
        [SerializeField] private Transform _container;

        [SerializeField] private List<DataA> _a;
        [SerializeField] private List<bool> _b;

        [field: SerializeField] public List<CellView> ParentCells { get; private set; }

        private int Width => _rotationType == RotationType.Top ||
                             _rotationType == RotationType.Bottom
            ? _width
            : _height;

        private int Height => _rotationType == RotationType.Top ||
                              _rotationType == RotationType.Bottom
            ? _height
            : _width;

        public int CellsCountForItem => Width * Height;
        private const int UpSortingOrder = 1;
        private const float SmallOffset = .015f;

        private int _defaultSortingOrder;
        private RotationType _rotationType;
        private RotationType _previousRotationType;

        public void Init(int defaultSortingOrder, float distanceBetweenCells)
        {
            _defaultSortingOrder = defaultSortingOrder + UpSortingOrder;
            _distanceBetweenCells = distanceBetweenCells;
            ResetOrder();
        }

        public void BeginDrag()
        {
            _previousRotationType = _rotationType;
            _canvas.sortingOrder = _defaultSortingOrder + UpSortingOrder;
        }

        public List<Vector2> GetPositions()
        {
            CalculateBounds(out Vector2 startPoint, out Vector2 _);
            Vector2 startPointCell = GetStartPointCell(startPoint);

            return GetCellsPositions(startPointCell);
        }

        public Vector2 GetPosition(Vector2 firstCellPosition)
        {
            //note: исходя из размера айтама и позиции первой ячейки вычесляем позицию для айтама
            firstCellPosition.x += (float)(Width - 1) / 2 * _distanceBetweenCells;
            firstCellPosition.y -= (float)(Height - 1) / 2 * _distanceBetweenCells;

            return firstCellPosition;
        }

        public void ResetOrder() =>
            _canvas.sortingOrder = _defaultSortingOrder;

        public void ChangeCell(List<CellView> cellDatas) =>
            ParentCells = cellDatas;

        public void Rotate()
        {
            var newType = (int)_rotationType + 1;
            newType = newType > (int)RotationType.Left ? 0 : newType;
            _rotationType = (RotationType)newType;

            Rotation();
        }

        public void ResetRotation()
        {
            _rotationType = _previousRotationType;
            Rotation();
        }

        private void Rotation()
        {
            switch (_rotationType)
            {
                case RotationType.Top:
                    _container.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case RotationType.Right:
                    _container.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case RotationType.Bottom:
                    _container.eulerAngles = new Vector3(0, 0, -180);
                    break;
                case RotationType.Left:
                    _container.eulerAngles = new Vector3(0, 0, -270);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CalculateBounds(out Vector2 startPoint, out Vector2 endPoint)
        {
            Vector2 currentPosition = transform.position;
            Vector2 halfSizeItem = new Vector2(_distanceBetweenCells * Width / 2, _distanceBetweenCells * Height / 2);

            startPoint = new Vector2(currentPosition.x - halfSizeItem.x, currentPosition.y - halfSizeItem.y);
            endPoint = new Vector2(currentPosition.x + halfSizeItem.x, currentPosition.y + halfSizeItem.y);
        }

        private Vector2 GetStartPointCell(Vector2 startPoint)
        {
            Vector2 startPointCell = new Vector2(startPoint.x + _distanceBetweenCells / 2,
                startPoint.y + _distanceBetweenCells / 2);

            return startPointCell;
        }

        private List<Vector2> GetCellsPositions(Vector2 startPointCell)
        {
            List<Vector2> positions = new List<Vector2>(Width * Height);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Vector2 position = startPointCell;
                    position.x += _distanceBetweenCells * i + SmallOffset;
                    position.y += _distanceBetweenCells * j + SmallOffset;
                    positions.Add(position);
                }
            }

            return positions;
        }

        private void OnDrawGizmos()
        {
            if (!_debug)
                return;

            Color previousColor = Gizmos.color;
            Gizmos.color = Color.magenta;

            CalculateBounds(out Vector2 startPoint, out Vector2 endPoint);
            DrawBox(startPoint, endPoint);

            Vector2 startPointCell = GetStartPointCell(startPoint);
            List<Vector2> positions = GetCellsPositions(startPointCell);

            foreach (Vector2 position in positions)
            {
                Gizmos.DrawSphere(position, 10);
            }

            Gizmos.color = previousColor;
        }

        private void DrawBox(Vector2 startPoint, Vector2 endPoint)
        {
            Gizmos.DrawLine(startPoint, new Vector2(startPoint.x, endPoint.y));
            Gizmos.DrawLine(new Vector2(startPoint.x, endPoint.y), endPoint);
            Gizmos.DrawLine(endPoint, new Vector2(endPoint.x, startPoint.y));
            Gizmos.DrawLine(new Vector2(endPoint.x, startPoint.y), startPoint);
        }

        private enum RotationType
        {
            Top = 0,
            Right = 1,
            Bottom = 2,
            Left = 3
        }
    }
}