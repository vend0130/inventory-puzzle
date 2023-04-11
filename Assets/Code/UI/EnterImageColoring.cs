using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI
{
    public class EnterImageColoring : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _current;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _enterColor;
        
        private void OnDisable() =>
            _current.color = _defaultColor;

        public void OnPointerEnter(PointerEventData eventData) =>
            _current.color = _enterColor;

        public void OnPointerExit(PointerEventData eventData) =>
            _current.color = _defaultColor;
    }
}