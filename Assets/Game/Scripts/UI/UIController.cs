using SpaceInvaders.Common.State;
using SpaceInvaders.UI.Screens;
using SpaceInvaders.Util;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.UI
{
    public class UIController : MonoBehaviour
    {
        // Fields
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private LoadingScreen _loadingScreen;
        
        [SerializeField]
        private MainMenuScreen _mainMenuScreen;
        
        [SerializeField]
        private GameplayScreen _gameplayScreen;
        
        [SerializeField]
        private ResultsScreen _resultsScreen;
        
        [SerializeField]
        private HighScoresScreen _highScoresScreen;

        [SerializeField]
        private ControlsScreen _controlsScreen;

        private BaseScreen _activeScreen;

        [Inject]
        private Camera _camera;

        [Inject]
        private void Init()
        {
            _canvas.worldCamera = _camera;
            _canvas.sortingLayerName = SortingLayers.Overlay;
        }

        // Methods
        public void SetState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Loading:
                    SetActiveScreen(_loadingScreen);
                    break;
                case GameState.MainMenu:
                    SetActiveScreen(_mainMenuScreen);
                    break;
                case GameState.Gameplay:
                    SetActiveScreen(_gameplayScreen);
                    break;
                case GameState.Results:
                    SetActiveScreen(_resultsScreen);
                    break;
                case GameState.HighScores:
                    SetActiveScreen(_highScoresScreen);
                    break;
                case GameState.Controls:
                    SetActiveScreen(_controlsScreen);
                    break;
                default:
                    break;
            }
        }

        private void SetActiveScreen(BaseScreen screen)
        {
            if (_activeScreen != null)
            {
                _activeScreen.Hide();
            }
            _activeScreen = screen;
            _activeScreen.Show();
        }
    }
}