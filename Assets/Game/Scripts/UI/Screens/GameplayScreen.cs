using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class GameplayScreen : BaseScreen
    {
        public Button PauseButton;
        
        [Space]
        public StatItem ScoreItem;
        public StatItem WaveItem;
        public StatItem LivesItem;
        
        [Space]
        public GameObject PauseOverlayView;
        public UIButton ResumeButton;
        public UIButton ExitButton;

        protected override void OnInit()
        {
            ResumeButton.Text.text = Strings.RESUME_BUTTON_TEXT;
            ExitButton.Text.text = Strings.EXIT_BUTTON_TEXT;
            
            PauseButton.onClick.AddListener(OnPauseButtonClicked);
            ResumeButton.Button.onClick.AddListener(OnResumeButtonClicked);
            ExitButton.Button.onClick.AddListener(OnExitButtonClicked);

            ScoreItem.LabelText.text = Strings.STAT_SCORE;
            ScoreItem.ValueText.text = "0";
            
            WaveItem.LabelText.text = Strings.STAT_WAVE;
            WaveItem.ValueText.text = "0";
            
            LivesItem.LabelText.text = Strings.STAT_LIVES;
            LivesItem.ValueText.text = "0";
        }

        protected override void OnShow()
        {
            SetPauseOverlayVisible(false);
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            // @TODO mobile controls
        }

        private void OnPauseButtonClicked()
        {
            GameController.PauseGame();
        }
        
        private void OnResumeButtonClicked()
        {
            GameController.ResumeGame();
        }
        
        private void OnExitButtonClicked()
        {
            GameController.ExitGame();
        }

        // @TODO separate methods for each stat?
        public void SetGameStats(int score, int wave, int lives)
        {
            ScoreItem.ValueText.text = $"{score}";
            WaveItem.ValueText.text = $"{wave}";
            LivesItem.ValueText.text = $"{lives}";
        }

        public void SetPauseOverlayVisible(bool visible)
        {
            PauseOverlayView.SetActive(visible);
        }
    }
}