using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class MainMenuScreen : BaseScreen
    {
        public TMP_Text TitleText;
        
        [Space]
        public ScoreInfoItem ScoreInfoItemEnemy1;
        public ScoreInfoItem ScoreInfoItemEnemy2;
        public ScoreInfoItem ScoreInfoItemEnemy3;
        public ScoreInfoItem ScoreInfoItemEnemyUFO;

        [Space]
        public UIButton PlayButton;
        public UIButton HighScoresButton;
        public UIButton ControlsButton;

        [Space]
        public TMP_Text VersionText;
        
        protected override void OnInit()
        {
            TitleText.text = Strings.GAME_TITLE;
            VersionText.text = $"v{Application.version}";
            
            ScoreInfoItemEnemy1.SetPointsValue(GameConfig.EnemiesConfig.GetScoreValueForEnemyType(EnemyType.Enemy1));
            ScoreInfoItemEnemy2.SetPointsValue(GameConfig.EnemiesConfig.GetScoreValueForEnemyType(EnemyType.Enemy2));
            ScoreInfoItemEnemy3.SetPointsValue(GameConfig.EnemiesConfig.GetScoreValueForEnemyType(EnemyType.Enemy3));
            ScoreInfoItemEnemyUFO.SetMysteryValue();
            
            PlayButton.Text.text = Strings.BUTTON_PLAY;
            HighScoresButton.Text.text = Strings.BUTTON_HIGH_SCORES;
            ControlsButton.Text.text = Strings.BUTTON_CONTROLS;
            
            PlayButton.Button.onClick.AddListener(OnPlayButtonClicked);
            HighScoresButton.Button.onClick.AddListener(OnHighScoresButtonClicked);
            ControlsButton.Button.onClick.AddListener(OnControlsButtonClicked);

            var isMobilePlatform = AppController.IsMobilePlatform();
            ControlsButton.gameObject.SetActive(!isMobilePlatform);
        }
        
        private void OnPlayButtonClicked()
        {
            GameController.StartGame();
        }

        private void OnHighScoresButtonClicked()
        {
            GameController.ShowHighScores();
        }

        private void OnControlsButtonClicked()
        {
            GameController.ShowControls();
        }
    }
}