using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class MainMenuScreen : BaseScreen
    {
        // @TODO convert to public
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
                    var pointsValue = GameplayConfig.EnemiesConfig.GetScoreValueForEnemyType(scoreInfoItem.EnemyType);
                    scoreInfoItem.SetPointsValue(pointsValue);
                }
            }
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            // @TODO for mobile
            _exitButton.gameObject.SetActive(false);
            _controlsButton.gameObject.SetActive(false);
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }

        private void OnPlayButtonClicked()
        {
            GameController.SetState(GameState.Gameplay);
        }

        private void OnHighScoresButtonClicked()
        {
            GameController.SetState(GameState.HighScores);
        }

        private void OnControlsButtonClicked()
        {
            GameController.SetState(GameState.Controls);
        }
    }
}