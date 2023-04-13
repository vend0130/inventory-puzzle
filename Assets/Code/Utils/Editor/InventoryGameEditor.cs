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
        private LootInventory _lootInventory => _inventory.LootInventory;
        private CellView[] Cells => _inventory.LootInventory.Cells;

        private void OnEnable() =>
            _inventory = (InventoryGame)serializedObject.targetObject;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Load and Refresh"))
                Refresh();

            if (GUILayout.Button("Update Inventories"))
                _lootInventory.UpdateGrid(_inventory.Items);
        }

        private void Refresh()
        {
            float distance = _lootInventory
                .GetCurrentDistance(_lootInventory.LayoutGroup.cellSize.x + _lootInventory.LayoutGroup.spacing.x);

            _inventory.CreateArrayItems();
            _lootInventory.RefreshGrid();
            _lootInventory.CreateArray();

            LoadCells(distance);
            LoadItems(distance);

            for (int i = 0; i < Cells.Length; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(Cells[i]);

            for (int i = 0; i < _inventory.Items.Count; i++)
                PrefabUtility.RecordPrefabInstancePropertyModifications(_inventory.Items[i]);

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
            item.ChangeInventory(_lootInventory);

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
            if (!CellsHelper.TryEnterOnCell(_lootInventory, item, out cells))
                throw new Exception($"not correct position: {item.name}");

            if (CellsHelper.DropCellCount(cells, item.CellsCountForItem)
                != CellsHelper.DropCellCount(item.ParentCells, item.CellsCountForItem))
                throw new Exception($"not correct position item: {item.name}");

            foreach (ItemCellData cell in cells)
            {
                if (!cell.CellOnGrid.Free && cell.CellInItem.Activate)
                    throw new Exception($"{item.name} on" +
                                        $" {cell.CellOnGrid.Item}. Name cell: {cell.CellOnGrid.name}");
            }
        }
    }
}