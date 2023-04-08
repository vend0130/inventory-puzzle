using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using DG.Tweening;
using UnityEngine;

namespace Code.Game.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvasOrder;
        [SerializeField] private Transform _containerForRotation;
        [SerializeField] private float _distanceBetweenCells = 69f;
        [SerializeField] private bool _debug;

        [field: SerializeField, Space(20)] public int Width { get; private set; } = 1;
        [field: SerializeField] public int Height { get; private set; } = 1;
        [field: SerializeField, Space] public WidthData[] Grid { get; set; }

        [field: SerializeField, HideInInspector]
        public int CellsCountForItem { get; set; }

        [field: SerializeField, HideInInspector]
        public List<CellView> ParentCells { get; private set; }

        [SerializeField, HideInInspector] private int _defaultSortingOrder;

        private const int UpSortingOrder = 1;
        private const float DurationRotate = .05f;
        private const Ease EaseType = Ease.Linear;

        private Tween _rotationTween;
        private Vector3 _previousRotation;
        private Vector3 _currentRotation;

        private Vector2 _previousPosition;
        private Vector2 _targetPosition;

        private void OnDestroy() =>
            _rotationTween.SimpleKill();

        public void Init(int defaultSortingOrder, float distanceBetweenCells)
        {
            _defaultSortingOrder = defaultSortingOrder + UpSortingOrder;
            _distanceBetweenCells = distanceBetweenCells;

            ResetOrder();
        }

        public void BeginDrag()
        {
            _targetPosition = _previousPosition = transform.position;
            _currentRotation = _previousRotation = _containerForRotation.eulerAngles;
            _canvasOrder.sortingOrder = _defaultSortingOrder + UpSortingOrder;
        }

        public List<Vector2> GetPositions()
        {
            RotationType rotationType = ItemHelper.GetRotationType(_currentRotation.z);

            ItemHelper.CalculateBounds(rotationType, transform.position, _distanceBetweenCells,
                new Vector2Int(Width, Height), out Vector2 startPoint, out Vector2 _);

            Vector2 startPointCell = ItemHelper.GetStartPointCell(rotationType, startPoint, _distanceBetweenCells);

            return ItemHelper.GetCellsPositions(rotationType, Grid, _distanceBetweenCells, startPointCell);
        }

        public Vector2 GetPosition() =>
            _targetPosition;

        public void ResetOrder() =>
            _canvasOrder.sortingOrder = _defaultSortingOrder;

        public void ChangeCell(List<CellView> cellDatas) =>
            ParentCells = cellDatas;

        public bool TryRotation()
        {
            if (_rotationTween != null && _rotationTween.active)
                return false;

            _currentRotation = _containerForRotation.eulerAngles;
            _currentRotation.z = _currentRotation.z + 90 >= 360 ? 0 : _currentRotation.z + 90;
            Rotation(_currentRotation);

            return true;
        }

        public void ResetRotation()
        {
            Rotation(_previousRotation);
            _targetPosition = _previousPosition;
        }

        public void ChangeOffset(Vector2 offset) =>
            _targetPosition = (Vector2)transform.position + offset;

        private void Rotation(Vector3 target)
        {
            _rotationTween.SimpleKill();
            _rotationTween = _containerForRotation
                .DORotate(target, DurationRotate)
                .SetEase(EaseType);
        }

        #region Gizmo

        private void OnDrawGizmos()
        {
            if (!_debug)
                return;

            Color previousColor = Gizmos.color;

            RotationType rotationType = ItemHelper.GetRotationType(_currentRotation.z);

            ItemHelper.CalculateBounds(rotationType, transform.position, _distanceBetweenCells,
                new Vector2Int(Width, Height), out Vector2 startPoint, out Vector2 endPoint);
            DrawBorder(startPoint, endPoint);

            if (Grid != null)
                DrawCells(rotationType, startPoint);

            Gizmos.color = previousColor;
        }

        private void DrawCells(RotationType rotationType, Vector2 startPoint)
        {
            Vector2 startPointCell = ItemHelper.GetStartPointCell(rotationType, startPoint, _distanceBetweenCells);
            List<Vector2> positions = ItemHelper.GetCellsPositions(rotationType, Grid,
                _distanceBetweenCells, startPointCell);

            Gizmos.color = Color.magenta;
            foreach (Vector2 position in positions)
                Gizmos.DrawSphere(position, 10);
        }

        private void DrawBorder(Vector2 startPoint, Vector2 endPoint)
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawLine(startPoint, new Vector2(startPoint.x, endPoint.y));
            Gizmos.DrawLine(new Vector2(startPoint.x, endPoint.y), endPoint);
            Gizmos.DrawLine(endPoint, new Vector2(endPoint.x, startPoint.y));
            Gizmos.DrawLine(new Vector2(endPoint.x, startPoint.y), startPoint);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPoint, 7);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endPoint, 7);
        }

        #endregion
    }
}