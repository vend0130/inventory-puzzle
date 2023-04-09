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
                Refresh();

            if (GUILayout.Button("Update Grid"))
                _lootInventory.UpdateGrid();
        }

        private void Refresh()
        {
            float distance =
                _lootInventory.GetCurrentDistance(_lootInventory.LayoutGroup.cellSize.x +
                                                  _lootInventory.LayoutGroup.spacing.x);

            _lootInventory.RefreshGrid();
            _lootInventory.CreateArray();

            LoadCells(distance);
            LoadItems(distance);

            for (int i = 0; i < Cells.Length; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(Cells[i]);

            for (int i = 0; i < _lootInventory.Items.Count; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(_lootInventory.Items[i]);

            Canvas.ForceUpdateCanvases();
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private void LoadCells(float distance)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = _lootInventory.ParentForCells.GetChild(i).GetComponent<CellView>();
                Cells[i].Init();
                Cells[i].RecalculatePoints(distance);
            }
        }

        private void LoadItems(float distance)
        {
            for (int i = 0; i < _lootInventory.CanvasWithItems.transform.childCount; i++)
            {
                var item = _lootInventory.CanvasWithItems.transform.GetChild(i).GetComponent<ItemView>();
                item.Init(_lootInventory.CanvasWithItems.sortingOrder);
                item.ChangeDistance(distance);

                _lootInventory.Items.Add(item);
            }

            foreach (var item in _lootInventory.Items)
            {
                if (CellsHelper.TryEnterOnCell(_lootInventory, item, out List<CellView> cells))
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
    }
}