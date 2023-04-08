using Code.Game.Cells;
using Code.Game.Inventory;
using UnityEditor;
using UnityEngine;

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
                _lootInventory.Refresh();
            }
        }
    }
}