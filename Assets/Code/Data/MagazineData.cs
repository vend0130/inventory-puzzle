using System.Collections.Generic;
using System.Globalization;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(MagazineData), menuName = "Static Data/" + nameof(MagazineData))]
    public class MagazineData : ScriptableObject
    {
        [field: SerializeField, ReadOnly, Space(15)]
        public string NameName { get; private set; } = "Название";

        [field: SerializeField] public string Name { get; private set; } = "Магазин";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameType { get; private set; } = "Тип";

        [field: SerializeField] public string Type { get; private set; } = "Магазин для";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameCalibre { get; private set; } = "Калибр";

        [field: SerializeField] public float Calibre { get; private set; } = 3.22f;

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameСapacity { get; private set; } = "Объем, патронов";

        [field: SerializeField] public int Сapacity { get; private set; } = 322;

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameMaterial { get; private set; } = "Материал";

        [field: SerializeField] public string Material { get; private set; } = "Vtnfk";

        public List<(string, string)> GetTexts()
        {
            return new List<(string, string)>(7)
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