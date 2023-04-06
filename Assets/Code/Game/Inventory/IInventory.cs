using Code.Game.Cells;

namespace Code.Game.Inventory
{
    public interface IInventory
    {
        public CellView[] Cells { get; }
    }
}