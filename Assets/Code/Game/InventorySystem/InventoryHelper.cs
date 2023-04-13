using UnityEngine;

namespace Code.Game.InventorySystem
{
    public static class InventoryHelper
    {
        public static bool Collision(Vector2 position, Rect rect) =>
            position.x >= rect.min.x && position.x < rect.max.x &&
            position.y >= rect.min.y && position.y < rect.max.y;
    }
}