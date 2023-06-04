using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace Code.Data.Items
{
    [CreateAssetMenu(fileName = nameof(SimpleData), menuName = "Static Data/Items/" + nameof(SimpleData))]
    public class SimpleData : ScriptableObject
    {
        //Name
        [field: SerializeField, Space(15)] public LocalizedString NameName { get; private set; } = "item.name";
        [field: SerializeField] public LocalizedString Name { get; private set; }

        //Description
        [field: SerializeField, Space(15)]
        public LocalizedString NameDescription { get; private set; } = "item.description";

        [field: SerializeField] public LocalizedString Description { get; private set; }

        public List<(LocalizedString, LocalizedString)> GetTexts()
        {
            return new List<(LocalizedString, LocalizedString)>(7)
            {
                (NameName, Name),
                (NameDescription, Description),
            };
        }
    }
}