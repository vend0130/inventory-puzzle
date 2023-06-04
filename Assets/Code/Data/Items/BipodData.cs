using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace Code.Data.Items
{
    [CreateAssetMenu(fileName = nameof(BipodData), menuName = "Static Data/Items/" + nameof(BipodData))]
    public class BipodData : ScriptableObject
    {
        [field: SerializeField, Space(15)] public LocalizedString NameName { get; private set; } = "item.name";
        [field: SerializeField] public LocalizedString Name { get; private set; }


        [field: SerializeField, Space(15)]
        public LocalizedString NameDescription { get; private set; } = "item.description";

        [field: SerializeField] public LocalizedString Description { get; private set; }


        [field: SerializeField, Space(15)] public LocalizedString NameType { get; private set; } = "item.type";
        [field: SerializeField] public LocalizedString Type { get; private set; }

        public List<(LocalizedString, LocalizedString)> GetTexts()
        {
            return new List<(LocalizedString, LocalizedString)>(3)
            {
                (NameName, Name),
                (NameDescription, Description),
                (NameType, Type),
            };
        }
    }
}