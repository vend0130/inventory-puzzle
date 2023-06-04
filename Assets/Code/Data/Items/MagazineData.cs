using System.Collections.Generic;
using System.Globalization;
using I2.Loc;
using UnityEngine;

namespace Code.Data.Items
{
    [CreateAssetMenu(fileName = nameof(MagazineData), menuName = "Static Data/Items/" + nameof(MagazineData))]
    public class MagazineData : ScriptableObject
    {
        //Name
        [field: SerializeField, Space(15)] public LocalizedString NameName { get; private set; } = "item.name";
        [field: SerializeField] public LocalizedString Name { get; private set; }

        //Type
        [field: SerializeField, Space(15)] public LocalizedString NameType { get; private set; } = "item.type";
        [field: SerializeField] public LocalizedString Type { get; private set; }

        //Calibre
        [field: SerializeField, Space(15)] public LocalizedString NameCalibre { get; private set; } = "item.calibre";
        [field: SerializeField] public float Calibre { get; private set; } = 3.22f;

        //Сapacity
        [field: SerializeField, Space(15)] public LocalizedString NameСapacity { get; private set; } = "item.capacity";
        [field: SerializeField] public int Сapacity { get; private set; } = 322;

        //Material
        [field: SerializeField, Space(15)] public LocalizedString NameMaterial { get; private set; } = "item.material";
        [field: SerializeField] public LocalizedString Material { get; private set; }

        public List<(LocalizedString, LocalizedString)> GetTexts()
        {
            return new List<(LocalizedString, LocalizedString)>(7)
            {
                (NameName, Name),
                (NameType, Type),
                (NameCalibre, Calibre.ToString(CultureInfo.InvariantCulture)),
                (NameСapacity, Сapacity.ToString()),
                (NameMaterial, Material),
            };
        }
    }
}