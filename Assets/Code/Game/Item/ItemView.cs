﻿using System.Collections.Generic;
using Code.Data;
using Code.Extensions;
using Code.Game.Cells;
using DG.Tweening;
using UnityEngine;

namespace Code.Game.Item
{
    public class ItemView : MonoBehaviour
    {
        [field: SerializeField] public GunData Data { get; private set; }
        [SerializeField] private Canvas _canvasOrder;
        [SerializeField] private Transform _containerForRotation;
        [SerializeField] public float _distanceBetweenCells = 69f;

        [field: SerializeField, Space(20)] public int Width { get; private set; } = 1;
        [field: SerializeField] public int Height { get; private set; } = 1;
        [field: SerializeField, Space] public WidthData[] Grid { get; set; }

        [field: SerializeField, HideInInspector]
        public int CellsCountForItem { get; set; }

        [field: SerializeField, HideInInspector]
        public List<CellView> ParentCells { get; private set; }

        [SerializeField, HideInInspector] private int _defaultSortingOrder;
        [SerializeField, HideInInspector] private Vector3 _currentRotation;

        [field: SerializeField, HideInInspector]
        public Vector2 LastOffset { get; private set; }

        public float DistanceBetweenCells => _distanceBetweenCells;
        public Vector3 CurrentRotation => _currentRotation;

        private const int UpSortingOrder = 1;
        private const float DurationRotate = .05f;
        private const Ease EaseType = Ease.Linear;

        private Tween _rotationTween;
        private Vector3 _previousRotation;
        private Vector2 _previousPosition;
        private Vector2 _targetPosition;

        private void Awake()
        {
            _targetPosition = _previousPosition = transform.position;
            _currentRotation = _previousRotation = _containerForRotation.eulerAngles;
        }

        private void OnDestroy() =>
            _rotationTween.SimpleKill();

        public void Init(int defaultSortingOrder)
        {
            _defaultSortingOrder = defaultSortingOrder + UpSortingOrder;
            _currentRotation = _containerForRotation.eulerAngles;

            ResetOrder();
        }

        public void ChangeDistance(float distanceBetweenCells) =>
            _distanceBetweenCells = distanceBetweenCells;

        public void BeginDrag()
        {
            _targetPosition = _previousPosition = transform.position;
            _currentRotation = _previousRotation = _containerForRotation.eulerAngles;
            _canvasOrder.sortingOrder = _defaultSortingOrder + UpSortingOrder;
        }

        public void ChangeLastOffset(Vector2 lastOffset) =>
            LastOffset = lastOffset;

        public List<Vector2> GetCellsPositions()
        {
            GetData(out RotationType rotationType, out Vector2 startPointCell);

            return ItemHelper.GetCellsPositions(rotationType, Grid, DistanceBetweenCells, startPointCell);
        }

        public Vector2 GetTargetPosition() =>
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
            _currentRotation = _previousRotation;
        }

        public void ChangeOffset() =>
            _targetPosition = (Vector2)transform.position + LastOffset;

        private void GetData(out RotationType rotationType, out Vector2 startPointCell)
        {
            rotationType = ItemHelper.GetRotationType(_currentRotation.z);

            ItemHelper.CalculateBounds(rotationType, transform.position, DistanceBetweenCells,
                new Vector2Int(Width, Height), out Vector2 startPoint, out Vector2 _);

            startPointCell = ItemHelper.GetStartPointCell(rotationType, startPoint, DistanceBetweenCells);
        }

        private void Rotation(Vector3 target)
        {
            _rotationTween.SimpleKill();
            _rotationTween = _containerForRotation
                .DORotate(target, DurationRotate)
                .SetEase(EaseType);
        }
    }
}