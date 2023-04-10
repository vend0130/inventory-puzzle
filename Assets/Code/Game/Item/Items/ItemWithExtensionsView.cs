using Code.Data;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemWithExtensionsView : BaseItem
    {
        [field: SerializeField] public GunData Data { get; private set; }

        public override void OpenMenu(Vector2 position) => 
            _itemMenu.Open(position);

        protected override void OpenInfo() =>
            _itemInfo.Open(Data);
    }
}