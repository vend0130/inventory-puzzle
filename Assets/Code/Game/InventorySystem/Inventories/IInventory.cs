using Code.Game.Cells;

namespace Code.Game.InventorySystem.Inventories
{
    public interface IInventory
    {
        public CellView[] Cells { get; }
    }
}