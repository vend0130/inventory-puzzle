using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item.Items;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.InventorySystem
{
    public class DragItems : MonoBehaviour
    {
        //TODO: rework when added another inventories
        [SerializeField] private LootInventory _inventory;

        private const float DurationMove = .1f;

        private BaseItem _item;
        private Vector2 _offset;
        private List<CellView> _previousDragCells = new List<CellView>();
        private Tween _tween;
        private bool _isEnabled = true;

        private void OnDestroy() =>
            _tween.SimpleKill();

        public void LeftDown(PointerEventData eventData)
        {
            if (!_isEnabled)
                return;

            if (CellsHelper.TryTapOnCell(_inventory, eventData.position, out CellView cell))
                BeginDrag(cell.Item, eventData);
            else
                _item = null;
        }

        public void RightClick(PointerEventData eventData)
        {
            if (!_isEnabled || _item != null)
                return;

            if (CellsHelper.TryTapOnCell(_inventory, eventData.position, out CellView cell))
                cell.Item.OpenMenu(eventData.position);
        }

        public void RightDown()
        {
            if (!_isEnabled || _item == null)
                return;

            RotationItem();
        }

        public void Drag(PointerEventData eventData)
        {
            if (_item == null || !_isEnabled)
                return;

            DragItem(eventData.position);
            ChangeCellsWhenDragItem();
        }

        public void Up()
        {
            if (_item == null || !_isEnabled)
                return;

            //note: if drop in new cells updated parent cells for item
            if (CellsHelper.TryDropInNewCells(_previousDragCells, _item.CellsCountForItem))
                _item.ChangeCell(_previousDragCells.Clone());
            else
                _item.ResetRotation();

            EndDrag();
        }

        private void BeginDrag(BaseItem item, PointerEventData eventData)
        {
            _tween.SimpleKill();
            PreviousCellsExit();

            _item = item;
            _item.BeginDrag();
            _item.ParentCells.ForEach((cell) => cell.RemoveItem());

            _offset = (Vector2)_item.transform.position - eventData.position;

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
                if (cell.Free)
                    cell.Enter();
                else
                    cell.BadEnter();
            }
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

            _item.ParentCells.ForEach((cell) => cell.AddItem(_item));
        }

        private void EndItemMove()
        {
            _item.ResetOrder();
            _item = null;
            _isEnabled = true;
        }

        private void RotationItem()
        {
            if (_item == null)
                return;

            if (_item.TryRotation())
                ChangeCellsWhenDragItem();
        }

        private void PreviousCellsEnter() =>
            _previousDragCells.ForEach((cell) => cell.Enter());

        private void PreviousCellsBadEnter() =>
            _previousDragCells.ForEach((cell) => cell.BadEnter());

        private void PreviousCellsExit() =>
            _previousDragCells.ForEach((cell) => cell.Exit());
    }
}