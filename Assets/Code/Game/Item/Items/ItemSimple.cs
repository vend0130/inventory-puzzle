using Code.Data;
using Code.Data.Items;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemSimple : BaseItem
    {
        [field: SerializeField] public SimpleData Data { get; private set; }

        public override void OpenInfo() =>
            ItemInfo.Open(Data);
    }
}