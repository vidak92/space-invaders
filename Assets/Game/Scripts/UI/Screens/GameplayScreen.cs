using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class GameplayScreen : BaseScreen
    {
        public Button PauseButton;
        
        public StatItem ScoreItem;
        public StatItem WaveItem;
        public StatItem LivesItem;

        protected override void OnInit()
        {
            PauseButton.onClick.AddListener(OnExitButtonClicked);

            ScoreItem.LabelText.text = Strings.STAT_SCORE;
            ScoreItem.ValueText.text = "0";
            
            WaveItem.LabelText.text = Strings.STAT_WAVE;
            WaveItem.ValueText.text = "0";
            
            LivesItem.LabelText.text = Strings.STAT_LIVES;
            LivesItem.ValueText.text = "0";
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            // @TODO mobile controls
        }

        private void OnExitButtonClicked()
        {
            GameController.SetState(GameState.MainMenu);
        }

        // @TODO separate methods for each stat?
        public void SetGameStats(int score, int wave, int lives)
        {
            ScoreItem.ValueText.text = $"{score}";
            WaveItem.ValueText.text = $"{wave}";
            LivesItem.ValueText.text = $"{lives}";
        }
    }
}