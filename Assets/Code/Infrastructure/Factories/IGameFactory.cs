using Code.Game.ItemInfo;
using Code.UI;

namespace Code.Infrastructure.Factories
{
    public interface IGameFactory
    {
        GamePlayUI GamePlayUI { get; }
        ItemMenu ItemMenu { get; }
        
        void CreateGamePlayUI();
        void CreateBackground();
        void CreateInfoPanel();
    }
}