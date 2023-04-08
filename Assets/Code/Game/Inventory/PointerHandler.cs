using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Inventory
{
    public class PointerHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action<PointerEventData> LeftDownHandler;
        public event Action RightDownHandler;
        public event Action<PointerEventData> DragHandler;
        public event Action UpHandler;

        private int? _pointerId;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                RightDownHandler?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId != null || eventData.button != PointerEventData.InputButton.Left)
                return;

            _pointerId = eventData.pointerId;
            LeftDownHandler?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId)
                return;

            DragHandler?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId)
                return;

            End();
        }

        private void OnApplicationPause(bool pauseStatus) =>
            End();

        private void End()
        {
            UpHandler?.Invoke();
            _pointerId = null;
        }
    }
}