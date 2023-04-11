using System;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Game.Item
{
    [Serializable]
    public class WidthData
    {
        [field: SerializeField] public CellInItemData[] Width { get; set; }
    }

    [Serializable]
    public class CellInItemData
    {
        [field: SerializeField, ReadOnly] public bool Value { get; set; }
        [field: SerializeField, ReadOnly] public ItemType Type { get; set; }
        [field: SerializeField, ReadOnly] public bool Activate { get; set; }

        public CellInItemData(bool value, ItemType type)
        {
            Value = value;
            Type = type;
            Activate = value;
        }
    }
}