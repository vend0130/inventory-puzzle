using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace Code.Data.Items
{
    [CreateAssetMenu(fileName = nameof(AimData), menuName = "Static Data/Items/" + nameof(AimData))]
    public class AimData : ScriptableObject
    {
        [field: SerializeField, Space(33)] public LocalizedString NameName { get; private set; } = "item.name";

        [field: SerializeField] public LocalizedString Name { get; private set; }

        [field: SerializeField, Space(15)]
        public LocalizedString NameDescription { get; private set; } = "item.description";

        [field: SerializeField] public LocalizedString Description { get; private set; }

        public List<(LocalizedString, LocalizedString)> GetTexts()
        {
            return new List<(LocalizedString, LocalizedString)>(2)
            {
                (NameName, Name),
                (NameDescription, Description),
            };
        }
    }
}