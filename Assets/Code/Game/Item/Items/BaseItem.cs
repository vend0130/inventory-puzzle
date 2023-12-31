﻿using System.Collections.Generic;
using Code.Extensions;
using Code.Game.InventorySystem.Inventories;
using Code.Game.ItemInfo;
using DG.Tweening;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class BaseItem : MonoBehaviour
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }

        [SerializeField, Space] private Canvas _canvasOrder;
        [SerializeField] private Transform _containerForRotation;

        [SerializeField, Space] private float _distanceBetweenCells = 50f;
        [field: SerializeField, Space] public int Width { get; private set; } = 1;
        [field: SerializeField] public int Height { get; private set; } = 1;

        [field: SerializeField] public AdditionalData[] AdditionalDatas { get; private set; }

        [SerializeField, HideInInspector] private int _defaultSortingOrder;
        [SerializeField, HideInInspector] private Vector3 _currentRotation;

        [field: SerializeField, HideInInspector]
        public WidthData[] Grid { get; set; }

        [field: SerializeField, HideInInspector]
        public int DefaultCellsCountForItem { get; set; }

        [field: SerializeField, HideInInspector]
        public int AdditionalsCellsCountForItem { get; set; }

        [field: SerializeField, HideInInspector]
        public List<ItemCellData> ParentCells { get; private set; }

        [field: SerializeField, HideInInspector]
        public Vector2 LastOffset { get; private set; }

        [field: SerializeField, HideInInspector]
        public BaseInventory CurrentInventor { get; private set; }

        public int CellsCountForItem => DefaultCellsCountForItem + AdditionalsCellsCountForItem;
        public float DistanceBetweenCells => _distanceBetweenCells;
        public Vector3 CurrentRotation => _currentRotation;
        public BaseItem ParentItem { get; private set; }

        protected IInfo ItemInfo;

        private const int UpSortingOrder = 1;
        private const float DurationRotate = .05f;
        private const Ease EaseType = Ease.Linear;

        private ItemMenu _itemMenu;
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

        public void LoadItem(int defaultSortingOrder)
        {
            _defaultSortingOrder = defaultSortingOrder + UpSortingOrder;
            _currentRotation = _containerForRotation.eulerAngles;

            ResetOrder();
        }

        public void Init(ItemMenu itemMenu, IInfo itemInfo)
        {
            _itemMenu = itemMenu;
            ItemInfo = itemInfo;
        }

        public void ChangeInventory(BaseInventory inventory) =>
            CurrentInventor = inventory;

        public void AddParentItem(BaseItem item, ItemType type)
        {
            ItemType = type;
            ParentItem = item;
        }

        public void ChangeDistance(float distanceBetweenCells) =>
            _distanceBetweenCells = distanceBetweenCells;

        public void ChangeLastOffset(Vector2 lastOffset) =>
            LastOffset = lastOffset;

        public void ChangeOffset() =>
            _targetPosition = (Vector2)transform.position + LastOffset;

        public void OpenMenu(Vector2 position) =>
            _itemMenu.Open(position, this);

        public virtual void OpenInfo() =>
            Debug.LogError("not override method");

        public void BeginDrag()
        {
            _targetPosition = _previousPosition = transform.position;
            _currentRotation = _previousRotation = _containerForRotation.eulerAngles;
            UpOrder();
        }

        public void UpOrder() =>
            _canvasOrder.sortingOrder = _defaultSortingOrder + UpSortingOrder;

        public List<(Vector2, CellInItemData)> GetCellsPositions()
        {
            GetData(out RotationType rotationType, out Vector2 startPointCell);

            return ItemHelper.GetCellsPositions(rotationType, Grid, DistanceBetweenCells, startPointCell);
        }

        public Vector2 GetTargetPosition() =>
            _targetPosition;

        public void ResetOrder() =>
            _canvasOrder.sortingOrder = _defaultSortingOrder;

        public void ClearCells() =>
            ParentCells.Clear();

        public void ChangeCell(List<ItemCellData> cellDatas) =>
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

        public void ChangeAdditionalState(int index, bool activate)
        {
            AdditionalDatas[index].Activate = activate;
            AdditionalDatas[index].Image.enabled = activate;

            for (int i = 0; i < AdditionalDatas[index].Indexes.Count; i++)
            {
                Grid[AdditionalDatas[index].Indexes[i].y]
                    .Width[AdditionalDatas[index].Indexes[i].x].ChangeActivateState(activate);
            }


            UpdateAdditionalsCellsCount();
        }

        public void ChangeAdditionalState(ItemType type, bool activate)
        {
            AdditionalData data = null;

            foreach (AdditionalData additional in AdditionalDatas)
            {
                if (additional.Type == type)
                {
                    data = additional;
                    break;
                }
            }

            data.Activate = activate;
            data.Image.enabled = activate;

            for (int i = 0; i < data.Indexes.Count; i++)
                Grid[data.Indexes[i].y].Width[data.Indexes[i].x].ChangeActivateState(activate);

            UpdateAdditionalsCellsCount();
        }

        public bool TryGetCountCellsForAdditional(ItemType itemType, out int count)
        {
            foreach (AdditionalData additional in AdditionalDatas)
            {
                if (additional.Type == itemType)
                {
                    count = additional.Indexes.Count;
                    return true;
                }
            }

            count = 0;
            return false;
        }

        public bool AdditionalIsActivate(ItemType itemType)
        {
            foreach (AdditionalData additional in AdditionalDatas)
            {
                if (additional.Type == itemType)
                    return additional.Activate;
            }

            return false;
        }

        public void UpdateAdditionalsCellsCount()
        {
            AdditionalsCellsCountForItem = 0;
            foreach (AdditionalData additional in AdditionalDatas)
            {
                if (additional.Activate)
                    AdditionalsCellsCountForItem += additional.Indexes.Count;
            }
        }

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