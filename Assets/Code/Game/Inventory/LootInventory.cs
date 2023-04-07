using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.Item;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using GridLayoutGroup = UnityEngine.UI.GridLayoutGroup;
using Color = UnityEngine.Color;

namespace Code.Game.Inventory
{
    public class LootInventory : MonoBehaviour, IInventory
    {
        [SerializeField] private PointerHandler _pointerHandler;
        [SerializeField] private DragItems _dragItems;
        [SerializeField] private Canvas _canvasWithItems;

        [field: SerializeField] public RectTransform ParentForCells { get; private set; }
        [field: SerializeField] public CellView[] Cells { get; private set; }

        private void Awake()
        {
            _pointerHandler.DownHandler += _dragItems.Down;
            _pointerHandler.DragHandler += _dragItems.Drag;
            _pointerHandler.UpHandler += _dragItems.Up;
        }

        private void OnDestroy()
        {
            _pointerHandler.DownHandler -= _dragItems.Down;
            _pointerHandler.DragHandler -= _dragItems.Drag;
            _pointerHandler.UpHandler -= _dragItems.Up;
        }


#if UNITY_EDITOR
        [Space, SerializeField] private bool _drawLineInEditor;

        [ContextMenu("Refresh Grid")]
        private void Refresh()
        {
            GridLayoutGroup layoutGroup = ParentForCells.GetComponent<GridLayoutGroup>();

            RefreshGrid(layoutGroup);
            LoadCells(layoutGroup);
            List<ItemView> items = LoadItems(layoutGroup);

            for (int i = 0; i < Cells.Length; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(Cells[i]);

            for (int i = 0; i < items.Count; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(items[i]);

            Canvas.ForceUpdateCanvases();
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private void RefreshGrid(GridLayoutGroup layoutGroup)
        {
            layoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
            layoutGroup.enabled = false;
        }

        private void LoadCells(GridLayoutGroup layoutGroup)
        {
            Cells = new CellView[ParentForCells.childCount];

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = ParentForCells.GetChild(i).GetComponent<CellView>();
                Cells[i].Init(layoutGroup.cellSize.x + layoutGroup.spacing.x);
            }
        }

        private List<ItemView> LoadItems(GridLayoutGroup layoutGroup)
        {
            List<ItemView> items = new List<ItemView>(_canvasWithItems.transform.childCount);
            for (int i = 0; i < _canvasWithItems.transform.childCount; i++)
            {
                var item = _canvasWithItems.transform.GetChild(i).GetComponent<ItemView>();
                item.Init(_canvasWithItems.sortingOrder, layoutGroup.cellSize.x + layoutGroup.spacing.x);
                items.Add(item);
            }

            foreach (var item in items)
            {
                if (CellsChecker.TryEnterOnCell(this, item.GetPositions(),
                        out List<CellView> cells, out Vector2 cellPosition, out Vector2 itemCellPosition))
                {
                    if (cells.Count != item.CellsCountForItem)
                        throw new Exception("not correct position item: " + item.name);

                    item.ChangeOffset(cellPosition - itemCellPosition);

                    item.ChangeCell(cells.Clone());
                    item.ParentCells.ForEach((cell) => cell.AddItem(item));

                    item.transform.position = item.GetPosition();
                }
            }

            return items;
        }

        private void OnDrawGizmos()
        {
            if (!_drawLineInEditor || Cells == null || Cells.Length == 0)
                return;

            Color previousColor = Gizmos.color;

            for (int i = 0; i < Cells.Length; i++)
            {
                DrawBox(Cells[i]);
            }

            Gizmos.color = previousColor;
        }

        private void DrawBox(CellView cell)
        {
            Vector2 startPoint = cell.StartPoint;
            Vector2 endPoint = cell.EndPoint;
            Vector2 point = cell.CenterPoint;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPoint, point);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector2(startPoint.x, endPoint.y), point);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(endPoint, point);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(endPoint.x, startPoint.y), point);
        }
#endif
    }
}