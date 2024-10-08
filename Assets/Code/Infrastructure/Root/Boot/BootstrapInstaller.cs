﻿using System;
using Code.Data;
using Code.Data.Audio;
using Code.Data.Localize;
using Code.Infrastructure.Factories;
using Code.Infrastructure.Factories.Audio;
using Code.Infrastructure.Services.Ad;
using Code.Infrastructure.Services.Audio;
using Code.Infrastructure.Services.LoadScene;
using Code.Infrastructure.Services.Progress;
using Code.Infrastructure.Services.SaveLoad;
using Code.Infrastructure.StateMachine;
using Code.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Root.Boot
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private LevelsData _levelsData;
        [SerializeField] private LocalizeConfig _localizeConfig;
        [SerializeField] private AudioData _audioData;
        [SerializeField] private CurtainView _curtain;
        [SerializeField] private PlatformType _platformType;

        private void OnValidate()
        {
            if (_platformType == PlatformType.Yandex)
                Debug.LogError($"PlatformType <b><i>{_platformType}</i></b> is obsolete");
        }

        public override void InstallBindings()
        {
            BindStateMachine();
            BindServices();
            BindFactories();
            BindInstance();

            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        private void BindInstance()
        {
            Container.Bind<LevelsData>().FromInstance(_levelsData).AsSingle();
            Container.Bind<LocalizeConfig>().FromInstance(_localizeConfig).AsSingle();
            Container.Bind<AudioData>().FromInstance(_audioData).AsSingle();
        }

        private void BindFactories()
        {
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            Container.BindInterfacesTo<AudioSourceFactory>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<ExitState>().AsSingle();
        }

        private void BindServices()
        {
            BindProgressService();
            BindAdService();
            BindLoadSceneService();

            Container.BindInterfacesTo<AudioService>().AsSingle();
        }

        private void BindLoadSceneService()
        {
            Container.BindInterfacesTo<CurtainView>().FromInstance(_curtain).AsSingle();
            Container.BindInterfacesTo<LoadSceneService>().AsSingle();
        }

        private void BindProgressService()
        {
            Container.BindInterfacesTo<ProgressService>().AsSingle();

            if (_platformType == PlatformType.PlayerPrefs)
                Container.BindInterfacesTo<SaveLoadService>().AsSingle();
            else
                Container.BindInterfacesTo<SaveLoadYandexService>().AsSingle();
        }

        private void BindAdService()
        {
            if (_platformType == PlatformType.PlayerPrefs)
                Container.BindInterfacesTo<EditorAdService>().AsSingle();
            else
                Container.BindInterfacesTo<AdYandexService>().AsSingle();
        }

        public void Initialize() =>
            Container.Resolve<IGameStateMachine>().Enter<BootstrapState>();

        private enum PlatformType
        {
            PlayerPrefs = 0,
            Yandex = 1
        }
    }
}