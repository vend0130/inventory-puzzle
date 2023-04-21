using Code.Data;
using Code.Data.Items;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemAim : BaseItem
    {
        [field: SerializeField] public AimData Data { get; private set; }

        public override void OpenInfo() =>
            ItemInfo.Open(Data);
    }
}