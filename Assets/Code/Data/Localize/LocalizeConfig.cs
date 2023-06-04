using I2.Loc;
using UnityEngine;

namespace Code.Data.Localize
{
    [CreateAssetMenu(fileName = nameof(LocalizeConfig), menuName = "Static Data/" + nameof(LocalizeConfig))]
    public class LocalizeConfig : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Exit { get; private set; }
        [field: SerializeField] public LocalizedString Level { get; private set; }
        [field: SerializeField] public LocalizedString Inventory { get; private set; }
        [field: SerializeField] public LocalizedString Loot { get; private set; }

        [field: SerializeField] public ItemMenuLocalizeConfig ItemMenu { get; private set; }
    }
}