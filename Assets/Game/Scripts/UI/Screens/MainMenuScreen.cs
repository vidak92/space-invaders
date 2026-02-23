using UnityEngine;

namespace SpaceInvaders
{
    public class MainMenuScreen : BaseScreen
    {
        public ScoreInfoItem ScoreInfoItemEnemy1;
        public ScoreInfoItem ScoreInfoItemEnemy2;
        public ScoreInfoItem ScoreInfoItemEnemy3;
        public ScoreInfoItem ScoreInfoItemEnemyUFO;

        public UIButton PlayButton;
        public UIButton HighScoresButton;
        public UIButton ControlsButton;
        
        protected override void OnInit()
        {
            PlayButton.Button.onClick.AddListener(OnPlayButtonClicked);
            HighScoresButton.Button.onClick.AddListener(OnHighScoresButtonClicked);
            ControlsButton.Button.onClick.AddListener(OnControlsButtonClicked);

            PlayButton.Text.text = Strings.PLAY_BUTTON_TEXT;
            HighScoresButton.Text.text = Strings.HIGH_SCORES_BUTTON_TEXT;
            ControlsButton.Text.text = Strings.CONTROLS_BUTTON_TEXT;

            ScoreInfoItemEnemy1.SetPointsValue(GameConfig.EnemiesConfig.GetScoreValueForEnemyType(EnemyType.Enemy1));
            ScoreInfoItemEnemy2.SetPointsValue(GameConfig.EnemiesConfig.GetScoreValueForEnemyType(EnemyType.Enemy1));
            ScoreInfoItemEnemy3.SetPointsValue(GameConfig.EnemiesConfig.GetScoreValueForEnemyType(EnemyType.Enemy1));
            ScoreInfoItemEnemyUFO.SetMysteryValue();
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            // @TODO for mobile
            ControlsButton.gameObject.SetActive(true);
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