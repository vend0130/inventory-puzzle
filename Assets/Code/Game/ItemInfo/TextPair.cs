using TMPro;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class TextPair : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI First { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Second { get; private set; }
    }
}