using Code.Data;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemGunView : BaseItem
    {
        [field: SerializeField] public GunData Data { get; private set; }

        protected override void OpenInfo() =>
            ItemInfo.Open(Data);
    }
}