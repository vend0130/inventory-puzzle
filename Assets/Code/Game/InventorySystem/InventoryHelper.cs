using Code.Game.InventorySystem.Inventories;
using UnityEngine;

namespace Code.Game.InventorySystem
{
    public static class InventoryHelper
    {
        public static bool TryGetInventory(Vector2 position, BaseInventory[] inventories, out BaseInventory inventory)
        {
            for (int i = 0; i < inventories.Length; i++)
            {
                if (Collision(position, inventories[i].Rect))
                {
                    inventory = inventories[i];
                    return true;
                }
            }

            inventory = null;
            return false;
        }

        private static bool Collision(Vector2 position, Rect rect) =>
            position.x >= rect.min.x && position.x < rect.max.x &&
            position.y >= rect.min.y && position.y < rect.max.y;
    }
}