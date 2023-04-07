using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.Item;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Inventory
{
    public class DragItems : MonoBehaviour
    {
        [SerializeField] private LootInventory _inventory;

        private const float DurationMove = .1f;

        private ItemView _currentItem;
        private Vector2 _offset;
        private List<CellView> _previousDragCells = new List<CellView>();
        private Tween _tween;

        //TODO: rework
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                RotationItem();
        }

        private void OnDestroy() =>
            _tween.SimpleKill();

        public void Down(PointerEventData eventData)
        {
            if (CellsChecker.TryTapOnCell(_inventory, eventData.position, out CellView cell))
                BeginDrag(cell.Item, eventData);
        }

        public void Drag(PointerEventData eventData)
        {
            if (_currentItem == null)
                return;

            DragItem(eventData.position);
            ChangeCellsWhenDragItem();
        }

        public void Up()
        {
            if (_currentItem == null)
                return;

            //note: if drop in new cells updated parent cells for item
            if (CellsChecker.TryDropInNewCells(_previousDragCells, _currentItem.CellsCountForItem))
                _currentItem.ChangeCell(_previousDragCells.Clone());
            else
                _currentItem.ResetRotation();

            EndDrag();
        }

        private void BeginDrag(ItemView item, PointerEventData eventData)
        {
            _currentItem = item;
            _currentItem.BeginDrag();
            _currentItem.ParentCells.ForEach((cell) => cell.RemoveItem());

            _offset = (Vector2)_currentItem.transform.position - eventData.position;

            _previousDragCells = _currentItem.ParentCells;
            PreviousCellsEnter();
        }

        private void DragItem(Vector2 position) =>
            _currentItem.transform.position = position + _offset;

        private void ChangeCellsWhenDragItem()
        {
            if (CellsChecker.TryEnterOnCell(_inventory, _currentItem.GetPositions(),
                    out List<CellView> cells, out Vector2 cellPosition, out Vector2 itemCellPosition))
            {
                _currentItem.ChangeOffset(cellPosition - itemCellPosition);

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
            if (cells.Count != _currentItem.CellsCountForItem)
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
            PreviousCellsExit();
            _previousDragCells.Clear();

            _tween.SimpleKill();

            Vector2 target = _currentItem.GetPosition();

            //float distance = Vector2.Distance(_item.transform.localPosition, _item.ParentCell.Point); => distance / 1000
            _tween = _currentItem.transform.DOMove(target, DurationMove)
                .SetEase(Ease.Linear)
                .OnComplete(EndItemMove);

            _currentItem.ParentCells.ForEach((cell) => cell.AddItem(_currentItem));

            enabled = false;
        }

        private void EndItemMove()
        {
            _currentItem.ResetOrder();
            _currentItem = null;
            enabled = true;
        }

        private void RotationItem()
        {
            if (_currentItem == null)
                return;

            _currentItem.Rotate();
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