using SpaceInvaders.Common;
using SpaceInvaders.Util;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI
{
    public class MainMenuScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _highScoresButton;

        [SerializeField]
        private Button _controlsButton;

        // Overrides
        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _highScoresButton.onClick.AddListener(OnHighScoresButtonClicked);
            _controlsButton.onClick.AddListener(OnControlsButtonClicked);
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            _exitButton.gameObject.SetActive(Utils.IsCurrentPlatformStandalone);
            _controlsButton.gameObject.SetActive(Utils.IsCurrentPlatformStandalone);
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }

        private void OnPlayButtonClicked()
        {
            _gameManager.SetState(GameState.Gameplay);
        }

        private void OnHighScoresButtonClicked()
        {
            _gameManager.SetState(GameState.HighScores);
        }

        private void OnControlsButtonClicked()
        {
            _gameManager.SetState(GameState.Controls);
        }
    }
}