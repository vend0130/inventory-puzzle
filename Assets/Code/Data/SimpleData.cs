using System.Collections.Generic;
using Code.Utils.Readonly;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(SimpleData), menuName = "Static Data/" + nameof(SimpleData))]
    public class SimpleData : ScriptableObject
    {
        [field: SerializeField, ReadOnly, Space(15)]
        public string NameName { get; private set; } = "Название";

        [field: SerializeField] public string Name { get; private set; } = "Магазин";

        [field: SerializeField, ReadOnly, Space(15)]
        public string NameDescription { get; private set; } = "Описание";

        [field: SerializeField, TextArea(5, 5)]
        public string Description { get; private set; } = "Клалаш";

        public List<(string, string)> GetTexts()
        {
            return new List<(string, string)>(7)
            {
                (NameName, Name),
                (NameDescription, Description),
            };
        }
    }
}