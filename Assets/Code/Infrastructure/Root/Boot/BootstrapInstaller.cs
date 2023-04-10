using Code.Infrastructure.Factories;
using Code.Infrastructure.Services.LoadScene;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Boot
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private CurtainView _curtain;

        public override void InstallBindings()
        {
            BindStateMachine();
            BindLoadScene();
            
            Container.BindInterfacesTo<GameFactory>().AsSingle();

            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<ExitState>().AsSingle();
        }

        private void BindLoadScene()
        {
            Container.BindInterfacesTo<CurtainView>().FromInstance(_curtain).AsSingle();
            Container.BindInterfacesTo<LoadSceneService>().AsSingle();
        }

        public void Initialize() =>
            Container.Resolve<IGameStateMachine>().Enter<LoadSceneState, string>(Constants.MainSceneName);
    }
}