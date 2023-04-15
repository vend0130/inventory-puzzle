using System.Collections.Generic;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(BipodData), menuName = "Static Data/" + nameof(BipodData))]
    public class BipodData : ScriptableObject
    {
        [field: SerializeField, ReadOnly, Space(15)]
        public string NameName { get; private set; } = "Название";

        [field: SerializeField] public string Name { get; private set; } = "сошки//";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameDescription { get; private set; } = "Описание";

        [field: SerializeField, TextArea(5, 5)]
        public string Description { get; private set; } = "Клалаш";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameType { get; private set; } = "Тип";

        [field: SerializeField] public string Type { get; private set; } = "сошки для";

        public List<(string, string)> GetTexts()
        {
            return new List<(string, string)>(7)
            {
                (NameName, Name),
                (NameDescription, Description),
                (NameType, Type),
            };
        }
    }
}