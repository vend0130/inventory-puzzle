using I2.Loc;
using UnityEngine;

namespace Code.Game.ItemInfo
{
    public class LocalizePair : MonoBehaviour
    {
        [field: SerializeField] public Localize First { get; private set; }
        [field: SerializeField] public Localize Second { get; private set; }
    }
}