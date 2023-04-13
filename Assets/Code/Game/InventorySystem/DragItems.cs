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
        private List<ItemCellData> _previousDragCells = new List<ItemCellData>();
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
                CellsHelper.DropCellCount(_item.ParentCells, _item.CellsCountForItem), _item, out CellView dropCell);

            if (dropType == DropType.Combine)
            {
                Combine(dropCell.Item);
                return;
            }

            if (dropType == DropType.Drop)
                _item.ChangeCell(new List<ItemCellData>(_previousDragCells));
            else
            {
                if (_isSpawned)
                {
                    SpawnedItemDestroy();
                    return;
                }

                _item.ResetRotation();
            }

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
            if (CellsHelper.TryEnterOnCell(_inventory, _item, out List<ItemCellData> cells))
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

        private void EnterCellsWhenDragItem(List<ItemCellData> cells)
        {
            if (CellsHelper.DropCellCount(cells, _item.CellsCountForItem) !=
                CellsHelper.DropCellCount(_item.ParentCells, _item.CellsCountForItem))
            {
                PreviousCellsBadEnter();
                return;
            }

            
            //TODO: окрашивать все клетки одним цветом
            foreach (var cell in cells)
            {
                if (!cell.CellInItem.Activate)
                    continue;

                if (!cell.CellOnGrid.Free && cell.CellOnGrid.Item.CombineItem(_item.ItemType))
                    cell.CellOnGrid.CombineEnter();
                else if (cell.CellOnGrid.Free)
                    cell.CellOnGrid.Enter();
                else
                    cell.CellOnGrid.BadEnter();
            }
        }

        private void Combine(BaseItem item)
        {
            PreviousCellsExit();
            _previousDragCells.Clear();

            item.ChangeAdditionalState(_item.ItemType, true);

            RemoveItemInCell(item.ParentCells);
            if (CellsHelper.TryEnterOnCell(item.CurrentInventor, item, out List<ItemCellData> cells))
                item.ChangeCell(cells);
            AddItemInCell(item.ParentCells, item);

            DestroyItemHandler?.Invoke(_item);
            Destroy(_item.gameObject);
            EndItemMove();
        }

        private void SpawnedItemDestroy()
        {
            PreviousCellsExit();
            _previousDragCells.Clear();

            BaseItem item = _item.ParentItem;

            item.ChangeAdditionalState(_item.ItemType, true);

            RemoveItemInCell(item.ParentCells);
            if (CellsHelper.TryEnterOnCell(item.CurrentInventor, item, out List<ItemCellData> cells))
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

        private void AddItemInCell(List<ItemCellData> cells, BaseItem item) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.AddItem(item);
            });

        private void RemoveItemInCell(List<ItemCellData> cells) =>
            cells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.RemoveItem();
            });

        private void PreviousCellsEnter() =>
            _previousDragCells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.Enter();
            });

        private void PreviousCellsBadEnter() =>
            _previousDragCells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.BadEnter();
            });

        private void PreviousCellsExit() =>
            _previousDragCells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.Exit();
            });
    }
}