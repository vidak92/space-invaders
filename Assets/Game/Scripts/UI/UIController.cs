using SpaceInvaders.Common;
using UnityEngine;

namespace SpaceInvaders.UI
{
    public class UIController : MonoBehaviour
    {
        // Fields
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

        private BaseScreen _activeScreen;

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