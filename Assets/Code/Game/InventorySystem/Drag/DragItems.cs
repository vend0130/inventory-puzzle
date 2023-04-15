using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item;
using Code.Game.Item.Items;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Game.InventorySystem.Drag
{
    public class DragItems : MonoBehaviour
    {
        [SerializeField] private Cells _cells;

        public event Action ItemEndMoveHandler;
        public event Action<BaseItem> DestroyItemHandler;

        private const float DurationMove = .1f;

        private BaseItem _item;
        private bool _isSpawned = false;
        private Vector2 _offset;
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
        }

        public void LeftDown(Vector2 position)
        {
            if (!_isEnabled)
                return;

            if (!_cells.TryGetInventory(position, out BaseInventory inventory))
                return;

            if (_cells.TryTapOnCell(inventory, position, out CellView cell))
                BeginDrag(cell.Item, position);
            else
                _item = null;
        }

        public void RightClick(Vector2 position)
        {
            if (!_isEnabled || _item != null)
                return;

            if (!_cells.TryGetInventory(position, out BaseInventory inventory))
                return;

            if (_cells.TryTapOnCell(inventory, position, out CellView cell))
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
            _cells.ChangeCellsWhenDragItem(_item);
        }

        public void Up()
        {
            if (_item == null || !_isEnabled)
                return;

            switch (_cells.GetDropType(_item, out BaseItem combineItem))
            {
                case DropType.Combine:
                    Combine(combineItem);
                    return;
                case DropType.Drop:
                    _item.ChangeCell(new List<ItemCellData>(_cells.PreviousDragCells));
                    break;
                case DropType.FailDrop:
                    if (_isSpawned)
                    {
                        SpawnedItemDestroy();
                        return;
                    }

                    _item.ResetRotation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EndDrag();
        }

        private void BeginDrag(BaseItem item, Vector2 position)
        {
            _tween.SimpleKill();
            _cells.CellsExit();

            _item = item;
            _item.BeginDrag();
            _item.ParentCells.RemoveItemInCell();

            _offset = (Vector2)_item.transform.position - position;

            _cells.ChangeCells(_item.ParentCells);
            _cells.CellsEnter();
        }

        private void DragItem(Vector2 position) =>
            _item.transform.position = position + _offset;

        private void Combine(BaseItem item)
        {
            _cells.ClearPreviousDragCells();

            item.ChangeAdditionalState(_item.ItemType, true);

            item.ParentCells.RemoveItemInCell();

            if (_cells.TryEnterOnCell(item, out List<ItemCellData> cells))
                item.ChangeCell(cells);

            item.ParentCells.AddItemInCell(item);

            DestroyItemHandler?.Invoke(_item);
            Destroy(_item.gameObject);
            EndItemMoveToPoint();
        }

        private void SpawnedItemDestroy()
        {
            _cells.ClearPreviousDragCells();

            BaseItem item = _item.ParentItem;

            item.ChangeAdditionalState(_item.ItemType, true);

            item.ParentCells.RemoveItemInCell();
            if (_cells.TryEnterOnCell(item, out List<ItemCellData> cells))
                item.ChangeCell(cells);

            item.ParentCells.AddItemInCell(item);

            DestroyItemHandler?.Invoke(_item);
            Destroy(_item.gameObject);

            EndItemMoveToPoint();
        }

        private void EndDrag()
        {
            _isEnabled = false;
            _cells.ClearPreviousDragCells();

            _tween.SimpleKill();

            Vector2 target = _item.GetTargetPosition();

            _tween = _item.transform.DOMove(target, DurationMove)
                .SetEase(Ease.Linear)
                .OnComplete(EndItemMoveToPoint);

            _item.ParentCells.AddItemInCell(_item);

            if (!_cells.TryGetInventory(_item.transform.position, out BaseInventory inventory))
                inventory = _item.CurrentInventor;

            _item.ChangeInventory(inventory);
        }

        private void EndItemMoveToPoint()
        {
            _tween.SimpleKill();
            _item.ResetOrder();

            _isSpawned = false;
            _isEnabled = true;

            if (_cells.TryGetInventory(_item.transform.position, out BaseInventory inventory))
                _item.ChangeInventory(inventory);

            _item = null;

            ItemEndMoveHandler?.Invoke();
        }

        private void RotationItem()
        {
            if (_item == null)
                return;

            if (_item.TryRotation())
                _cells.ChangeCellsWhenDragItem(_item);
        }
    }
}