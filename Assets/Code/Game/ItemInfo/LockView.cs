using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Game.ItemInfo
{
    public class LockView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _current;
        [SerializeField] private Button _closeButton;

        public event Action CloseHandler;

        private void Awake()
        {
            _closeButton.Add(Close);
            _current.SetActive(false);
        }

        private void OnDestroy() =>
            _closeButton.Remove(Close);

        public void On() =>
            _current.SetActive(true);

        public void OnPointerDown(PointerEventData eventData) =>
            Close();

        private void Close()
        {
            CloseHandler?.Invoke();
            _current.SetActive(false);
        }
    }
}