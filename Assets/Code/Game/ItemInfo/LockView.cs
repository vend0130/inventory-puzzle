using System;
using System.Collections.Generic;
using Code.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Game.ItemInfo
{
    public class LockView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _current;
        [SerializeField] private List<Button> _closeButtons;

        public event Action CloseHandler;

        private void Awake()
        {
            foreach (Button button in _closeButtons)
                button.Add(Close);

            _current.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach (Button button in _closeButtons)
                button.Remove(Close);
        }

        public void OnPointerDown(PointerEventData _) =>
            Close();

        public void On() =>
            _current.SetActive(true);

        public void Close()
        {
            CloseHandler?.Invoke();
            _current.SetActive(false);
        }
    }
}