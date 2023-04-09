using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.ItemInfo
{
    [Serializable]
    public class MenuButtonData : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
    }
}