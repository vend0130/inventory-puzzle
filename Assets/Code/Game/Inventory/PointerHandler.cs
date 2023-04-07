using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Inventory
{
    public class PointerHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action<PointerEventData> DownHandler;
        public event Action<PointerEventData> DragHandler;
        public event Action UpHandler;

        private int? _pointerId;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId != null)
                return;

            _pointerId = eventData.pointerId;
            DownHandler?.Invoke(eventData);
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