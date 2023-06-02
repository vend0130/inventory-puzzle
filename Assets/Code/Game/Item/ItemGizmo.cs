using System.Collections.Generic;
using Code.Game.Item.Items;
using UnityEngine;

namespace Code.Game.Item
{
    public class ItemGizmo : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private BaseItem _item;
        [SerializeField] private bool _debug = true;

        private void OnDrawGizmos()
        {
            if (!_debug)
                return;

            Color previousColor = Gizmos.color;

            RotationType rotationType = ItemHelper.GetRotationType(_item.CurrentRotation.z);

            ItemHelper.CalculateBounds(rotationType, transform.position, _item.DistanceBetweenCells,
                new Vector2Int(_item.Width, _item.Height), out Vector2 startPoint, out Vector2 endPoint);
            DrawBorder(startPoint, endPoint);

            if (_item.Grid != null && _item.Grid.Length != 0)
                DrawCells(rotationType, startPoint);

            Gizmos.color = previousColor;
        }

        private void DrawCells(RotationType rotationType, Vector2 startPoint)
        {
            Vector2 startPointCell = ItemHelper.GetStartPointCell(rotationType, startPoint, _item.DistanceBetweenCells);
            List<(Vector2, CellInItemData)> positions = ItemHelper.GetCellsPositionsWithType(rotationType, _item.Grid,
                _item.DistanceBetweenCells, startPointCell);

            Gizmos.color = Color.magenta;
            foreach (var position in positions)
            {
                if (!position.Item2.Activate)
                    continue;

                Gizmos.color = position.Item2.Type == _item.ItemType ? Color.green : Color.yellow;
                Gizmos.DrawSphere(position.Item1, 10);
            }
        }

        private void DrawBorder(Vector2 startPoint, Vector2 endPoint)
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawLine(startPoint, new Vector2(startPoint.x, endPoint.y));
            Gizmos.DrawLine(new Vector2(startPoint.x, endPoint.y), endPoint);
            Gizmos.DrawLine(endPoint, new Vector2(endPoint.x, startPoint.y));
            Gizmos.DrawLine(new Vector2(endPoint.x, startPoint.y), startPoint);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startPoint, 7);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endPoint, 7);
        }
#endif
    }
}