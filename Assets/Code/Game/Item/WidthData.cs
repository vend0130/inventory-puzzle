using System;
using UnityEngine;

namespace Code.Game.Item
{
    [Serializable]
    public class WidthData
    {
        [field: SerializeField] public bool[] Width { get; set; }
    }
}