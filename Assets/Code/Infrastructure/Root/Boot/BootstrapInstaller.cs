using Code.Data;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Services.LoadScene;
using Code.Infrastructure.Services.Progress;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Boot
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private LevelsData _levelsData;
        [SerializeField] private CurtainView _curtain;

        public override void InstallBindings()
        {
            BindStateMachine();
            BindLoadScene();
            BindProgressService();

            Container.Bind<LevelsData>().FromInstance(_levelsData).AsSingle();

            Container.BindInterfacesTo<GameFactory>().AsSingle();

            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<ExitState>().AsSingle();
        }

        private void BindLoadScene()
        {
            Container.BindInterfacesTo<CurtainView>().FromInstance(_curtain).AsSingle();
            Container.BindInterfacesTo<LoadSceneService>().AsSingle();
        }

        private void BindProgressService()
        {
            Container.BindInterfacesTo<ProgressService>().AsSingle();
            Container.Bind<ProgressData>().AsSingle();
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;
            Container.Resolve<IGameStateMachine>().Enter<BootstrapState>();
        }
    }
}