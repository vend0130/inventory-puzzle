using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = nameof(GunData), menuName = "Static Data/" + nameof(GunData))]
    public class GunData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "Клалаш";
        [field: SerializeField, TextArea] public string Description { get; private set; } = "Клалаш";
        [field: SerializeField] public float Calibre { get; private set; } = 3.22f;
        [field: SerializeField] public float Size { get; private set; } = 666f;
        [field: SerializeField] public float SightingRange { get; private set; } = 666f;
        [field: SerializeField] public float MaxDistance { get; private set; } = 666f;
        [field: SerializeField] public string TypeArmo { get; private set; } = "магазин";
    }
}