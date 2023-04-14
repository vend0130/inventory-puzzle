using System;
using System.Collections.Generic;
using Code.Extensions;
using Code.Game.Cells;
using Code.Game.InventorySystem;
using Code.Game.InventorySystem.Inventories;
using Code.Game.Item;
using Code.Game.Item.Items;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Utils.Editor
{
    [CustomEditor(typeof(InventoryGame))]
    public class InventoryGameEditor : UnityEditor.Editor
    {
        private InventoryGame _inventory;
        private bool DrawButtonsInMainInventory = true;

        private LootInventory LootInventory => _inventory.LootInventory;
        private MainInventory MainInventory => _inventory.MainInventory;
        private CellView[] LootCells => LootInventory.Cells;
        private CellView[] MainCells => MainInventory.Cells;

        private void OnEnable() =>
            _inventory = (InventoryGame)serializedObject.targetObject;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawButtonsInMainInventory = GUILayout.Toggle(DrawButtonsInMainInventory, "DrawButtonsInMainInventory");

            if (GUILayout.Button("Update"))
                Refresh();
        }

        private void OnSceneGUI()
        {
            if (!DrawButtonsInMainInventory)
                return;

            Draw();
            Event e = Event.current;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Vector2 position = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
                if (TryGetCell(position, out CellView cell))
                {
                    cell.ChangeStateLock(!cell.Lock);
                    SceneView.RepaintAll();
                }
            }
        }

        private void Draw()
        {
            for (int i = 0; i < MainCells.Length; i++)
            {
                if (MainCells[i].Lock)
                {
                    Handles.color = Color.red;
                    Handles.DrawSolidDisc(MainCells[i].CenterPoint, Vector3.forward, 10);
                }
            }
        }

        private bool TryGetCell(Vector2 position, out CellView cell)
        {
            for (int i = 0; i < MainCells.Length; i++)
            {
                if (CellsHelper.Collision(position, MainCells[i]))
                {
                    cell = MainCells[i];
                    return true;
                }
            }

            cell = null;
            return false;
        }

        private void Refresh()
        {
            float distance = LootInventory.GetCurrentDistance();

            ResetCells();
            _inventory.CreateArrayItems();
            LoadItems(distance);

            Save();
        }

        private void ResetCells()
        {
            for (int i = 0; i < LootCells.Length; i++)
                LootCells[i].Init();

            for (int i = 0; i < MainCells.Length; i++)
                MainCells[i].Init();

            LootInventory.ChangeRect();
            MainInventory.ChangeRect();
        }

        private void LoadItems(float distance)
        {
            for (int i = 0; i < _inventory.CanvasWithItems.transform.childCount; i++)
            {
                var item = _inventory.CanvasWithItems.transform.GetChild(i).GetComponent<BaseItem>();
                item.LoadItem(_inventory.CanvasWithItems.sortingOrder);
                item.ChangeDistance(distance);

                _inventory.Items.Add(item);
            }

            foreach (BaseItem item in _inventory.Items)
            {
                item.ClearCells();
                ChangeItem(item);
            }
        }

        private void ChangeItem(BaseItem item)
        {
            CheckItemPosition(item, out List<ItemCellData> cells);

            item.ChangeOffset();
            item.ChangeInventory(LootInventory);

            item.ChangeCell(cells.Clone());
            item.ParentCells.ForEach((cell) =>
            {
                if (cell.CellInItem.Activate)
                    cell.CellOnGrid.AddItem(item);
            });

            item.transform.position = item.GetTargetPosition();
        }

        private void CheckItemPosition(BaseItem item, out List<ItemCellData> cells)
        {
            if (!CellsHelper.TryEnterOnCell(LootInventory, item, out cells))
                throw new Exception($"not correct position: {item.name}");

            if (CellsHelper.DropCellCount(cells, item.CellsCountForItem)
                != CellsHelper.DropCellCount(item.ParentCells, item.CellsCountForItem))
                throw new Exception($"not correct position item: {item.name}");

            foreach (ItemCellData cell in cells)
            {
                if (!cell.CellOnGrid.Free && cell.CellInItem.Activate)
                    throw new Exception($"{item.name} on" +
                                        $" {cell.CellOnGrid.Item.name}. Name cell: {cell.CellOnGrid.name}");
            }
        }

        private void Save()
        {
            PrefabUtility.RecordPrefabInstancePropertyModifications(LootInventory);
            PrefabUtility.RecordPrefabInstancePropertyModifications(MainInventory);

            for (int i = 0; i < LootCells.Length; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(LootCells[i]);

            for (int i = 0; i < MainCells.Length; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(MainCells[i]);

            for (int i = 0; i < _inventory.Items.Count; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(_inventory.Items[i]);

            Canvas.ForceUpdateCanvases();
            if (!Application.isPlaying)
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
    }
}