using SpaceInvaders.Common.State;
using SpaceInvaders.Gameplay.Config;
using SpaceInvaders.Gameplay.Objects;
using SpaceInvaders.UI.Controls;
using SpaceInvaders.Util;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.UI.Screens
{
    public class MainMenuScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private ScoreInfoItem[] _scoreInfoItems;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _highScoresButton;

        [SerializeField]
        private Button _controlsButton;

        private GameplayConfig _gameplayConfig;

        [Inject]
        public void InitGameplayConfig(GameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;
        }

        // Overrides
        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _highScoresButton.onClick.AddListener(OnHighScoresButtonClicked);
            _controlsButton.onClick.AddListener(OnControlsButtonClicked);
        }

        protected override void OnShow()
        {
            foreach (var scoreInfoItem in _scoreInfoItems)
            {
                if (scoreInfoItem.EnemyType == EnemyType.UFO)
                {
                    scoreInfoItem.SetMysteryValue();
                }
                else
                {
                    var pointsValue = _gameplayConfig.EnemiesConfig.GetScoreValueForEnemyType(scoreInfoItem.EnemyType);
                    scoreInfoItem.SetPointsValue(pointsValue);
                }
            }
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
            _appController.SetState(GameState.Gameplay);
        }

        private void OnHighScoresButtonClicked()
        {
            _appController.SetState(GameState.HighScores);
        }

        private void OnControlsButtonClicked()
        {
            _appController.SetState(GameState.Controls);
        }
    }
}