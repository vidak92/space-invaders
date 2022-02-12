using SpaceInvaders.Gameplay;
using SpaceInvaders.UI;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Common
{
    public class DependencyInstaller : MonoInstaller
    {
        // Fields
        [SerializeField]
        private GameManager _gameManagerPrefab;

        [SerializeField]
        private UIController _uiControllerPrefab;
        
        [SerializeField]
        private GameplayController _gameplayController;
        
        [SerializeField]
        private GameplayConfig _gameplayConfig;

        [SerializeField]
        private GameplayAssetsConfig _gameplayAssetsConfig;

        // Methods
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(Camera.main);

            Container.Bind<GameManager>().FromComponentInNewPrefab(_gameManagerPrefab).AsSingle().NonLazy();
            Container.Bind<UIController>().FromComponentInNewPrefab(_uiControllerPrefab).AsSingle().NonLazy();
            Container.Bind<GameplayController>().FromComponentInNewPrefab(_gameplayController).AsSingle().NonLazy();
            Container.Bind<GameplayConfig>().FromScriptableObject(_gameplayConfig).AsSingle().NonLazy();
            Container.Bind<GameplayAssetsConfig>().FromScriptableObject(_gameplayAssetsConfig).AsSingle().NonLazy();

            Container.Bind<InputService>().AsSingle().NonLazy();
            Container.Bind<GameStatsController>().AsSingle().NonLazy();
            Container.Bind<HighScoreService>().AsSingle().NonLazy();

            Container.Bind<LoadingState>().AsSingle().NonLazy();
            Container.Bind<MainMenuState>().AsSingle().NonLazy();
            Container.Bind<GameplayState>().AsSingle().NonLazy();
            Container.Bind<ResultsState>().AsSingle().NonLazy();
            Container.Bind<HighScoresState>().AsSingle().NonLazy();
        }
    }
}