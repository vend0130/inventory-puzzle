using System.Collections.Generic;
using System.Globalization;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(GunData), menuName = "Static Data/" + nameof(GunData))]
    public class GunData : ScriptableObject
    {
        [field: SerializeField, ReadOnly] public string NameName { get; private set; } = "Название";
        [field: SerializeField] public string Name { get; private set; } = "Клалаш";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameDescription { get; private set; } = "Описание";

        [field: SerializeField, TextArea(5, 5)]
        public string Description { get; private set; } = "Клалаш";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameCalibre { get; private set; } = "Калибр";

        [field: SerializeField] public float Calibre { get; private set; } = 3.22f;

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameSize { get; private set; } = "Длина, мм";

        [field: SerializeField] public float Size { get; private set; } = 666f;

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameSightingRange { get; private set; } = "Прицельная дальность, м";

        [field: SerializeField] public float SightingRange { get; private set; } = 666f;

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameMaxDistance { get; private set; } = "максимальная дальность полёта пули, м";

        [field: SerializeField] public float MaxDistance { get; private set; } = 666f;

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameTypeArmo { get; private set; } = "Вид боепитания";

        [field: SerializeField] public string TypeArmo { get; private set; } = "магазин";

        public List<(string, string)> GetTexts()
        {
            return new List<(string, string)>(7)
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