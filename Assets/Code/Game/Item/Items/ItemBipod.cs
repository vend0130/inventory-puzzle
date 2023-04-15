using Code.Data;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemBipod : BaseItem
    {
        [field: SerializeField] public BipodData Data { get; private set; }

        public override void OpenInfo() =>
            ItemInfo.Open(Data);
    }
}