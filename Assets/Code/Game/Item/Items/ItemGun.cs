using Code.Data;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemGun : BaseItem
    {
        [field: SerializeField] public GunData Data { get; private set; }

        public override void OpenInfo() =>
            ItemInfo.Open(Data);
    }
}