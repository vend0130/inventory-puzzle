using Code.Game.Cells;
using Code.Game.InventorySystem.Inventories;
using UnityEditor;
using UnityEngine;

namespace Code.Utils.Editor
{
    [CustomEditor(typeof(BaseInventory), true)]
    public class InventoryEditor : UnityEditor.Editor
    {
        private BaseInventory _inventory;

        private void OnEnable() =>
            _inventory = (BaseInventory)serializedObject.targetObject;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Load Cells"))
                LoadCells();
        }

        private void LoadCells()
        {
            _inventory.RefreshGrid();
            _inventory.CreateArray();

            float distance = _inventory.GetCurrentDistance();

            for (int i = 0; i < _inventory.Cells.Length; i++)
            {
                _inventory.Cells[i] = _inventory.ParentForCells.GetChild(i).GetComponent<CellView>();
                _inventory.Cells[i].Init();
                _inventory.Cells[i].RecalculatePoints(distance);
            }
        }
    }
}