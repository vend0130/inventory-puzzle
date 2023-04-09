using Code.UI;

namespace Code.Infrastructure.Factories
{
    public interface IGameFactory
    {
        void CreateGamePlayUI();
        GamePlayUI GamePlayUI { get; }
        void CreateBackground();
    }
}