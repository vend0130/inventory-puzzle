using Code.Game.Item;
using Code.Property.Readonly;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Cells
{
    public class CellView : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public Vector2 CenterPoint { get; private set; }
        [field: SerializeField, ReadOnly] public Vector2 StartPoint { get; private set; }
        [field: SerializeField, ReadOnly] public Vector2 EndPoint { get; private set; }
        [field: SerializeField, ReadOnly] public bool Free { get; private set; }
        [field: SerializeField, ReadOnly] public ItemView Item { get; private set; }

        [Space, SerializeField] private Image _coloringImage;
        [SerializeField] private Color _freeColor;
        [SerializeField] private Color _busyColor;


        private Color _defaultColor;

        public void Init(float distanceBetweenCells)
        {
            _defaultColor = _coloringImage.color;

            CenterPoint = transform.position;
            StartPoint = GetStartPoint(CenterPoint, distanceBetweenCells);
            EndPoint = new Vector2(StartPoint.x + distanceBetweenCells, StartPoint.y + distanceBetweenCells);

            Free = true;
        }

        public void Enter() =>
            _coloringImage.color = _freeColor;

        public void BadEnter() =>
            _coloringImage.color = _busyColor;

        public void Exit() =>
            _coloringImage.color = _defaultColor;

        public void AddItem(ItemView item)
        {
            Free = false;
            Item = item;
        }

        public void RemoveItem()
        {
            Free = true;
            Item = null;
        }

        private Vector2 GetStartPoint(Vector2 defaultPosition, float distanceBetweenCells)
        {
            float halfCell = distanceBetweenCells / 2;
            return new Vector2(defaultPosition.x - halfCell, defaultPosition.y - halfCell);
        }
    }
}