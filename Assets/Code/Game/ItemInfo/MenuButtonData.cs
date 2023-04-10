using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Game.ItemInfo
{
    [Serializable]
    public class MenuButtonData : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }

        [SerializeField] private Image _current;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _enterColor;

        public event Action<int> DownHandler;

        private int _index;

        private void OnDisable() =>
            _current.color = _defaultColor;

        public void OnPointerDown(PointerEventData eventData) =>
            DownHandler?.Invoke(_index);

        public void OnPointerEnter(PointerEventData eventData) =>
            _current.color = _enterColor;

        public void OnPointerExit(PointerEventData eventData) =>
            _current.color = _defaultColor;

        public void ChangeIndex(int index)
        {
            _index = index;
        }
    }
}