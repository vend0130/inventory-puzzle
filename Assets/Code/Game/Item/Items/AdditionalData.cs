using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Item.Items
{
    [Serializable]
    public class AdditionalData
    {
        [field: SerializeField] public ItemType Type { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public bool Activate { get; private set; }
        [field: SerializeField] public AdditionalType AdditionalType { get; private set; }
    }
}