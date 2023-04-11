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
        [field: SerializeField] public bool Activate { get; set; }
        [field: SerializeField] public AdditionalType AdditionalType { get; private set; }
        [field: SerializeField] public Vector2Int Indexes { get; set; }
        [field: SerializeField] public GameObject Prefab { get; set; }
    }
}