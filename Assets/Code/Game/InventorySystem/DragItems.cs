using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item.Items;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Game.InventorySystem
{
    public class DragItems : MonoBehaviour
    {
        //TODO: rework when added another inventories
        [SerializeField] private LootInventory _inventory;

        public event Action<BaseItem> DropNewItemHandler;
        public event Action<BaseItem> DestroyItemHandler;

        private const float DurationMove = .1f;

        private BaseItem _item;
        private bool _isSpawned = false;
        private Vector2 _offset;
        private List<CellView> _previousDragCells = new List<CellView>();
        private Tween _tween;
        private bool _isEnabled = true;

        private void OnDestroy() =>
            _tween.SimpleKill();

        public void AddSpawnedItem(BaseItem item)
        {
            if (_item != null)
                Assert.IsNotNull(_item, "bug in logic");

            _isSpawned = true;
            _item = item;
            DropNewItemHandler?.Invoke(_item);
        }

        public void LeftDown(Vector2 position)
        {
            if (!_isEnabled)
                return;

            if (CellsHelper.TryTapOnCell(_inventory, position, out CellView cell))
                BeginDrag(cell.Item, position);
            else
                _item = null;
        }

        public void RightClick(Vector2 position)
        {
            if (!_isEnabled || _item != null)
                return;

            if (CellsHelper.TryTapOnCell(_inventory, position, out CellView cell))
                cell.Item.OpenMenu(position);
        }

        public void RightDown()
        {
            if (!_isEnabled || _item == null)
                return;

            RotationItem();
        }

        public void Drag(Vector2 position)
        {
            if (_item == null || !_isEnabled)
                return;

            DragItem(position);
            ChangeCellsWhenDragItem();
        }

        public void Up()
        {
            if (_item == null || !_isEnabled)
                return;

            DropType dropType = CellsHelper.TryDropInNewCells(_previousDragCells,
                _item.CellsCountForItem, _item.ItemType, out CellView dropCell);

            if (dropType == DropType.Combine)
            {
                Combine(dropCell.Item);
                return;
            }

            if (dropType == DropType.Drop)
                _item.ChangeCell(_previousDragCells.Clone());
            else
                FailDrop();

            EndDrag();
        }

        private void BeginDrag(BaseItem item, Vector2 position)
        {
            _tween.SimpleKill();
            PreviousCellsExit();
            _item = item;

            _item.BeginDrag();
            RemoveItemInCell(_item.ParentCells);

            _offset = (Vector2)_item.transform.position - position;

            _previousDragCells = _item.ParentCells;
            PreviousCellsEnter();
        }

        private void DragItem(Vector2 position) =>
            _item.transform.position = position + _offset;

        private void ChangeCellsWhenDragItem()
        {
            if (CellsHelper.TryEnterOnCell(_inventory, _item, out List<CellView> cells))
            {
                _item.ChangeOffset();

                PreviousCellsExit();
                _previousDragCells = cells;

                EnterCellsWhenDragItem(cells);
            }
            else
            {
                PreviousCellsExit();
                _previousDragCells.Clear();
            }
        }

        private void EnterCellsWhenDragItem(List<CellView> cells)
        {
            if (cells.Count != _item.CellsCountForItem)
            {
                PreviousCellsBadEnter();
                return;
            }

            foreach (CellView cell in cells)
            {
                if (!cell.Free && cell.Item.CombineItem(_item.ItemType))
                    cell.CombineEnter();
                else if (cell.Free)
                    cell.Enter();
                else
                    cell.BadEnter();
            }
        }

        private void Combine(BaseItem item)
        {
            PreviousCellsExit();
            _previousDragCells.Clear();

            item.ChangeAdditionalState(_item.ItemType, true);

            RemoveItemInCell(item.ParentCells);
            if (CellsHelper.TryEnterOnCell(item.CurrentInventor, item, out List<CellView> cells))
                item.ChangeCell(cells);
            AddItemInCell(item.ParentCells, item);

            DestroyItemHandler?.Invoke(_item);
            Destroy(_item.gameObject);
            EndItemMove();
        }

        private void FailDrop()
        {
            if (_isSpawned)
            {
                SpawnedItemDestroy();
                return;
            }

            _item.ResetRotation();
        }

        private void SpawnedItemDestroy()
        {
            PreviousCellsExit();
            _previousDragCells.Clear();

            BaseItem item = _item.ParentItem;

            item.ChangeAdditionalState(_item.ItemType, true);

            RemoveItemInCell(item.ParentCells);
            if (CellsHelper.TryEnterOnCell(item.CurrentInventor, item, out List<CellView> cells))
                item.ChangeCell(cells);
            AddItemInCell(item.ParentCells, item);

            DestroyItemHandler?.Invoke(_item);
            Destroy(_item.gameObject);
            EndItemMove();
        }

        private void EndDrag()
        {
            _isEnabled = false;
            PreviousCellsExit();
            _previousDragCells.Clear();

            _tween.SimpleKill();

            Vector2 target = _item.GetTargetPosition();

            _tween = _item.transform.DOMove(target, DurationMove)
                .SetEase(Ease.Linear)
                .OnComplete(EndItemMove);

            AddItemInCell(_item.ParentCells, _item);
        }

        private void EndItemMove()
        {
            _tween.SimpleKill();
            _item.ResetOrder();
            _item = null;
            _isEnabled = true;
            _isSpawned = false;
        }

        private void RotationItem()
        {
            if (_item == null)
                return;

            if (_item.TryRotation())
                ChangeCellsWhenDragItem();
        }

        private void AddItemInCell(List<CellView> cells, BaseItem item) =>
            cells.ForEach((cell) => cell.AddItem(item));

        private void RemoveItemInCell(List<CellView> cells) =>
            cells.ForEach((cell) => cell.RemoveItem());

        private void PreviousCellsEnter() =>
            _previousDragCells.ForEach((cell) => cell.Enter());

        private void PreviousCellsBadEnter() =>
            _previousDragCells.ForEach((cell) => cell.BadEnter());

        private void PreviousCellsExit() =>
            _previousDragCells.ForEach((cell) => cell.Exit());
    }
}