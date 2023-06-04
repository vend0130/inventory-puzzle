using System.Collections.Generic;
using System.Globalization;
using I2.Loc;
using UnityEngine;

namespace Code.Data.Items
{
    [CreateAssetMenu(fileName = nameof(GunData), menuName = "Static Data/Items/" + nameof(GunData))]
    public class GunData : ScriptableObject
    {
        //Name
        [field: SerializeField] public LocalizedString NameName { get; private set; } = "item.name";
        [field: SerializeField] public LocalizedString Name { get; private set; }

        //Description
        [field: SerializeField, Space(15)]
        public LocalizedString NameDescription { get; private set; } = "item.description";

        [field: SerializeField] public LocalizedString Description { get; private set; }

        //Calibre
        [field: SerializeField, Space(15)] public LocalizedString NameCalibre { get; private set; } = "item.calibre";
        [field: SerializeField] public float Calibre { get; private set; } = 3.22f;

        //Size
        [field: SerializeField, Space(15)] public LocalizedString NameSize { get; private set; } = "item.length";
        [field: SerializeField] public float Size { get; private set; } = 666f;

        //SightingRange
        [field: SerializeField, Space(15)]
        public LocalizedString NameSightingRange { get; private set; } = "item.sightingRange";

        [field: SerializeField] public float SightingRange { get; private set; } = 666f;

        //MaxDistance
        [field: SerializeField, Space(15)]
        public LocalizedString NameMaxDistance { get; private set; } = "item.maxDistance";

        [field: SerializeField] public float MaxDistance { get; private set; } = 666f;

        //TypeArmo
        [field: SerializeField, Space(15)] public LocalizedString NameTypeArmo { get; private set; } = "item.typeArmo";
        [field: SerializeField] public LocalizedString TypeArmo { get; private set; }

        public List<(LocalizedString, LocalizedString)> GetTexts()
        {
            return new List<(LocalizedString, LocalizedString)>(7)
            {
                (NameName, Name),
                (NameDescription, Description),
                (NameCalibre, Calibre.ToString(CultureInfo.InvariantCulture)),
                (NameSize, Size.ToString(CultureInfo.InvariantCulture)),
                (NameSightingRange, SightingRange.ToString(CultureInfo.InvariantCulture)),
                (NameMaxDistance, MaxDistance.ToString(CultureInfo.InvariantCulture)),
                (NameTypeArmo, TypeArmo),
            };
        }
    }
}