using Code.Game.Inventory;
using Code.Infrastructure.Factories;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInitialize : IInitializable
    {
        private readonly IGameFactory _gameFactory;
        private readonly DragItems _dragItems;

        public LevelInitialize(IGameFactory gameFactory, DragItems dragItems)
        {
            _gameFactory = gameFactory;
            _dragItems = dragItems;
        }

        public void Initialize()
        {
        }
    }
}