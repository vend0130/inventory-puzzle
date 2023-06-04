using System;
using I2.Loc;
using UnityEngine;

namespace Code.Data.Localize
{
    [Serializable]
    public class ItemMenuLocalizeConfig
    {
        [field: SerializeField] public LocalizedString Information { get; private set; }
        [field: SerializeField] public LocalizedString Take { get; private set; }
        [field: SerializeField] public LocalizedString Magazine { get; private set; }
        [field: SerializeField] public LocalizedString Bipods { get; private set; }
        [field: SerializeField] public LocalizedString Scope { get; private set; }
    }
}