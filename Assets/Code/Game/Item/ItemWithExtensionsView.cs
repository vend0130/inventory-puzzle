using Code.Data;
using UnityEngine;

namespace Code.Game.Item
{
    public class ItemWithExtensionsView : BaseItem
    {
        [field: SerializeField] public GunData Data { get; private set; }

        public override void OpenMenu(Vector2 position)
        {
            Debug.Log("OpenMenu");
        }

        public override void OpenInfo()
        {
            Debug.Log("OpenInfo");
        }
    }
}