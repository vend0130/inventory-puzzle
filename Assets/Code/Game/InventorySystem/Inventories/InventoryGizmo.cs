#if UNITY_EDITOR
using Code.Game.Cells;
using UnityEngine;

namespace Code.Game.InventorySystem.Inventories
{
    public class InventoryGizmo : MonoBehaviour
    {
        [Space, SerializeField] private bool _debug = false;

        private BaseInventory _inventory;

        private void OnDrawGizmos()
        {
            _inventory ??= GetComponent<BaseInventory>();

            if (!_debug || _inventory.Cells == null || _inventory.Cells.Length == 0)
                return;

            Color previousColor = Gizmos.color;

            for (int i = 0; i < _inventory.Cells.Length; i++)
            {
                if (_inventory.Cells[i].Lock)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(_inventory.Cells[i].CenterPoint, 10);
                }
                else
                {
                    DrawBox(_inventory.Cells[i]);
                }
            }

            Gizmos.color = previousColor;

            DrawRect();
        }

        private void DrawRect()
        {
            Gizmos.DrawSphere(_inventory.Rect.min, 10);
            Gizmos.DrawSphere(_inventory.Rect.max, 10);
            Gizmos.DrawSphere(new Vector2(_inventory.Rect.min.x, _inventory.Rect.max.y), 10);
            Gizmos.DrawSphere(new Vector2(_inventory.Rect.max.x, _inventory.Rect.min.y), 10);
        }

        private void DrawBox(CellView cell)
        {
            if (!cell.Free)
                return;

            Vector2 startPoint = cell.StartPoint;
            Vector2 endPoint = cell.EndPoint;
            Vector2 point = cell.CenterPoint;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPoint, point);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector2(startPoint.x, endPoint.y), point);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(endPoint, point);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(endPoint.x, startPoint.y), point);
        }
    }
}
#endif