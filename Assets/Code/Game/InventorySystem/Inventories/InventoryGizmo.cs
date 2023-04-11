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
                DrawBox(_inventory.Cells[i]);

            Gizmos.color = previousColor;
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