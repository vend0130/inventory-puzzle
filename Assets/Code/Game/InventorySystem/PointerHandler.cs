using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.InventorySystem
{
    public class PointerHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public Vector2 MousePosition => Input.mousePosition;

        public event Action<Vector2> LeftDownHandler;
        public event Action RightDownHandler;
        public event Action<Vector2> RightClickHandler;
        public event Action<Vector2> DragHandler;
        public event Action UpHandler;

        private int? _pointerId;
        private bool _mouseControl;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                RightDownHandler?.Invoke();

            if (!_mouseControl)
                return;

            if (Input.GetMouseButtonUp(0))
            {
                End();
                return;
            }

            DragHandler?.Invoke(Input.mousePosition);
        }

        private void OnApplicationPause(bool pauseStatus) =>
            End();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId != null || eventData.button != PointerEventData.InputButton.Left || _mouseControl)
                return;

            _pointerId = eventData.pointerId;
            LeftDownHandler?.Invoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_pointerId != eventData.pointerId || _mouseControl)
                return;

            DragHandler?.Invoke(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_mouseControl)
                return;

            if (_pointerId == null)
                RightClickHandler?.Invoke(eventData.position);

            if (_pointerId != eventData.pointerId)
                return;

            End();
        }

        //note: if menuItem click on button
        public void SetMouseDrag()
        {
            _mouseControl = true;
        }

        private void End()
        {
            UpHandler?.Invoke();
            _pointerId = null;
            _mouseControl = false;
        }
    }
}