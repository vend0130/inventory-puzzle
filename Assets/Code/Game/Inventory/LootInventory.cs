using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.Item;
using Code.Utils.Readonly;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Game.Inventory
{
    public class LootInventory : MonoBehaviour, IInventory
    {
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private DragItems _dragItems;
        [SerializeField] private Canvas _canvasWithItems;
        [SerializeField] private GridLayoutGroup _layoutGroup;
        [field: SerializeField] public RectTransform ParentForCells { get; private set; }
        [field: SerializeField, ReadOnly] public CellView[] Cells { get; private set; }
        [SerializeField, ReadOnly] private List<ItemView> _items;

        private Vector2Int _previousScreenSize;

        private void Awake()
        {
            _pointerHandler.DownHandler += _dragItems.Down;
            _pointerHandler.DragHandler += _dragItems.Drag;
            _pointerHandler.UpHandler += _dragItems.Up;

            _previousScreenSize = CellsHelper.CurrentSizeScreen();
            UpdateGrid();
        }

        //TODO: to main class
        private void Update()
        {
            if (CellsHelper.CurrentSizeScreen() != _previousScreenSize)
                UpdateGrid();

            _previousScreenSize = CellsHelper.CurrentSizeScreen();
        }

        private void OnDestroy()
        {
            _pointerHandler.DownHandler -= _dragItems.Down;
            _pointerHandler.DragHandler -= _dragItems.Drag;
            _pointerHandler.UpHandler -= _dragItems.Up;
        }

        public void Refresh()
        {
            float distance = GetCurrentDistance(_layoutGroup.cellSize.x + _layoutGroup.spacing.x);

            RefreshGrid();
            LoadCells(distance);
            LoadItems(distance);

            for (int i = 0; i < Cells.Length; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(Cells[i]);

            for (int i = 0; i < _items.Count; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(_items[i]);

            Canvas.ForceUpdateCanvases();
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private void UpdateGrid()
        {
            RefreshGrid();

            float distance = GetCurrentDistance(_layoutGroup.cellSize.x + _layoutGroup.spacing.x);

            for (int i = 0; i < Cells.Length; i++)
                Cells[i].RecalculatePoints(distance);

            foreach (var item in _items)
            {
                item.ChangeDistance(distance);
                item.ChangeLastOffset(item.ParentCells[0].CenterPoint - item.GetFirstCellPosition());
                item.ChangeOffset();
                item.transform.position = item.GetTargetPosition();
            }
        }

        private void RefreshGrid()
        {
            _layoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
            _layoutGroup.enabled = false;
        }

        private void LoadCells(float distance)
        {
            Cells = new CellView[ParentForCells.childCount];

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = ParentForCells.GetChild(i).GetComponent<CellView>();
                Cells[i].Init();
                Cells[i].RecalculatePoints(distance);
            }
        }

        private void LoadItems(float distance)
        {
            _items = new List<ItemView>(_canvasWithItems.transform.childCount);

            for (int i = 0; i < _canvasWithItems.transform.childCount; i++)
            {
                var item = _canvasWithItems.transform.GetChild(i).GetComponent<ItemView>();
                item.Init(_canvasWithItems.sortingOrder);
                item.ChangeDistance(distance);

                _items.Add(item);
            }

            foreach (var item in _items)
            {
                if (CellsHelper.TryEnterOnCell(this, item, out List<CellView> cells))
                {
                    if (cells.Count != item.CellsCountForItem)
                        throw new Exception("not correct position item: " + item.name);

                    item.ChangeOffset();

                    item.ChangeCell(cells.Clone());
                    item.ParentCells.ForEach((cell) => cell.AddItem(item));

                    item.transform.position = item.GetTargetPosition();
                }
            }
        }

        private float GetCurrentDistance(float distance) =>
            CellsHelper.GetCurrentDistance(distance);
    }
}