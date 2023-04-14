using Code.Game.InventorySystem;
using Code.Game.Item.Items;
using Code.Game.ItemInfo;
using Code.UI;
using UnityEngine;

namespace Code.Infrastructure.Factories
{
    public interface IGameFactory
    {
        GamePlayUI GamePlayUI { get; }
        ItemMenu ItemMenu { get; }
        DragItems DragItems { get; }
        PointerHandler PointerHandler { get; }
        InventoryGame InventoryGame { get; }

        void CreateGamePlayUI();
        void CreateBackground();
        void CreateInfoPanel();
        void CreateLevel();
        BaseItem CreateItem(BaseItem parentItem, int index, Vector2 position);
    }
}