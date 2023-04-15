using Code.Game.InventorySystem;
using Code.Game.InventorySystem.Drag;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Level
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private DragItems _dragItems;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LevelInitialize>().AsSingle();
            Container.Bind<DragItems>().FromInstance(_dragItems).AsSingle();
        }
    }
}