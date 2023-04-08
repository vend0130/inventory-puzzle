using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.Inventory;
using Code.Game.Item;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Utils.Editor
{
    [CustomEditor(typeof(LootInventory))]
    public class LootInventoryEditor : UnityEditor.Editor
    {
        private LootInventory _lootInventory;
        private CellView[] Cells => _lootInventory.Cells;

        private void OnEnable()
        {
            _lootInventory = (LootInventory)serializedObject.targetObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Refresh Grid And Items"))
            {
                GridLayoutGroup layoutGroup = _lootInventory.ParentForCells.GetComponent<GridLayoutGroup>();

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
        }

        private void RefreshGrid(GridLayoutGroup layoutGroup)
        {
            layoutGroup.enabled = true;
            Canvas.ForceUpdateCanvases();
            layoutGroup.enabled = false;
        }

        private void LoadCells(GridLayoutGroup layoutGroup)
        {
            _lootInventory.CreateCells();

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = _lootInventory.ParentForCells.GetChild(i).GetComponent<CellView>();
                Cells[i].Init(layoutGroup.cellSize.x + layoutGroup.spacing.x);
            }
        }

        private List<ItemView> LoadItems(GridLayoutGroup layoutGroup)
        {
            List<ItemView> items = new List<ItemView>(_lootInventory.CanvasWithItems.transform.childCount);
            for (int i = 0; i < _lootInventory.CanvasWithItems.transform.childCount; i++)
            {
                var item = _lootInventory.CanvasWithItems.transform.GetChild(i).GetComponent<ItemView>();
                item.Init(_lootInventory.CanvasWithItems.sortingOrder,
                    layoutGroup.cellSize.x + layoutGroup.spacing.x);
                items.Add(item);
            }

            foreach (var item in items)
            {
                if (CellsChecker.TryEnterOnCell(_lootInventory, item.GetPositions(),
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
    }
}