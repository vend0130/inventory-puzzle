using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.ItemInfo
{
    [Serializable]
    public class MenuButtonData : MonoBehaviour, IPointerDownHandler
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }

        public event Action<int> DownHandler;

        private int _index;

        public void OnPointerDown(PointerEventData _) =>
            DownHandler?.Invoke(_index);

        public void ChangeIndex(int index) =>
            _index = index;
    }
}