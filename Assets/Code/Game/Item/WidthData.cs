using System;
using Code.Game.Cells;
using UnityEngine;

namespace Code.Game.Item
{
    [Serializable]
    public class WidthData
    {
        [field: SerializeField] public CellInItemData[] Width { get; set; }
    }

    [Serializable]
    public class ItemCellData
    {
        [field: SerializeField] public CellView CellOnGrid { get; set; }
        [field: SerializeField] public CellInItemData CellInItem { get; set; }

        public ItemCellData(CellView cellOnGrid, CellInItemData cellInItem)
        {
            CellOnGrid = cellOnGrid;
            CellInItem = cellInItem;
        }
    }

    [Serializable]
    public class CellInItemData
    {
        [field: SerializeField] public bool Value { get; set; }
        [field: SerializeField] public ItemType Type { get; set; }
        [field: SerializeField] public bool Activate { get; private set; }

        public CellInItemData(bool value, ItemType type)
        {
            Value = value;
            Type = type;
            Activate = value;
        }

        public void ChangeActivateState(bool value) =>
            Activate = value;
    }
}