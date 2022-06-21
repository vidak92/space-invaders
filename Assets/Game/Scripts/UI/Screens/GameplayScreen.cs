using SpaceInvaders.Common.State;
using SpaceInvaders.Gameplay;
using SpaceInvaders.UI.Controls;
using SpaceInvaders.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI.Screens
{
    public class GameplayScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private TMP_Text _scoreText;

        [SerializeField]
        private TMP_Text _waveText;

        [SerializeField]
        private OnScreenButton _moveLeftButton;

        [SerializeField]
        private OnScreenButton _moveRightButton;

        [SerializeField]
        private TMP_Text _livesText;

        [SerializeField]
        private OnScreenButton _shootButton;

        // Overrides
        protected override void OnInit()
        {
            _gameStatsController.OnGameStatsUpdated += OnGameStatsUpdated;

            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            _moveLeftButton.gameObject.SetActive(Utils.IsCurrentPlatformMobile);
            _moveRightButton.gameObject.SetActive(Utils.IsCurrentPlatformMobile);
            _shootButton.gameObject.SetActive(Utils.IsCurrentPlatformMobile);
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            _gameStatsController.ResetStats();
            _appController.SetState(GameState.MainMenu);
        }

        private void OnGameStatsUpdated(GameStats gameStats)
        {
            _scoreText.text = $"SCORE: {gameStats.Score}";
            _waveText.text = $"WAVE: {gameStats.Wave}";
            _livesText.text = $"LIVES: {gameStats.Lives}";
        }
    }
}