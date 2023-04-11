using Code.Game.Cells;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Game.InventorySystem.Inventories
{
    public class BaseInventory : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public CellView[] Cells { get; private set; }
        [field: SerializeField] public RectTransform ParentForCells { get; private set; }

        public void CreateArray() =>
            Cells = new CellView[ParentForCells.childCount];
    }
}