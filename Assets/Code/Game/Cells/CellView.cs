﻿using Code.Game.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Cells
{
    public class CellView : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Point { get; private set; }
        [field: SerializeField] public Vector2 StartPoint { get; private set; }
        [field: SerializeField] public Vector2 EndPoint { get; private set; }
        [field: SerializeField] public bool Free { get; private set; }
        [field: SerializeField] public ItemView Item { get; private set; }

        [SerializeField] private Image _coloringImage;
        [SerializeField] private Color _freeColor;
        [SerializeField] private Color _busyColor;


        private Color _defaultColor;

        public void Init(float distanceBetweenCells)
        {
            _defaultColor = _coloringImage.color;

            Point = transform.position;
            StartPoint = GetStartPoint(Point, distanceBetweenCells);
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