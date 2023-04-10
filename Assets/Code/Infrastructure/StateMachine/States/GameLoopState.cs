using System;
using Code.Extensions;
using Code.Game.Item;
using Code.Game.Item.Items;
using Code.Infrastructure.Factories;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState, IDisposable
    {
        private readonly IGameFactory _gameFactory;
        private IGameStateMachine _stateMachine;

        public GameLoopState(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void InitStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter()
        {
            _gameFactory.GamePlayUI.AgainButton.Add(OnAgain);
            _gameFactory.GamePlayUI.ExitButton.Add(OnExit);

            _gameFactory.ItemMenu.CreateItemHandler += CreateItem;
        }

        public void Exit()
        {
            _gameFactory.GamePlayUI.AgainButton.Remove(OnAgain);
            _gameFactory.GamePlayUI.ExitButton.Remove(OnExit);

            _gameFactory.ItemMenu.CreateItemHandler -= CreateItem;
        }

        public void Dispose()
        {
            _gameFactory.GamePlayUI.AgainButton.RemoveAll();
        }

        private void CreateItem(ItemType itemType)
        {
            BaseItem item = _gameFactory.CreateItem(itemType, _gameFactory.PointerHandler.MousePosition);
            _gameFactory.DragItems.AddSpawnedItem(item);
            _gameFactory.PointerHandler.SetMouseDrag();
        }

        private void OnAgain() =>
            _stateMachine.Enter<LoadSceneState, string>(Constants.MainSceneName);

        private void OnExit() =>
            _stateMachine.Enter<ExitState>();
    }
}