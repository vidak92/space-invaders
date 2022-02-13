using SpaceInvaders.Common;
using SpaceInvaders.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI
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
            _moveLeftButton.Init(_inputService);
            _moveRightButton.Init(_inputService);
            _shootButton.Init(_inputService);

            _gameStatsController.OnGameStatsUpdated += OnGameStatsUpdated;

            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            _gameStatsController.ResetStats();
            _gameManager.SetState(GameState.MainMenu);
        }

        private void OnGameStatsUpdated(GameStats gameStats)
        {
            _scoreText.text = $"SCORE: {gameStats.Score}";
            _waveText.text = $"WAVE: {gameStats.Wave}";
            _livesText.text = $"LIVES: {gameStats.Lives}";
        }
    }
}