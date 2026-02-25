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

        [Space]
        public TouchButton ShootButton;
        public TouchJoystick MoveJoystick;

        protected override void OnInit()
        {
            ResumeButton.Text.text = Strings.RESUME_BUTTON_TEXT;
            ExitButton.Text.text = Strings.EXIT_BUTTON_TEXT;
            
            PauseButton.onClick.AddListener(OnPauseButtonClicked);
            ResumeButton.Button.onClick.AddListener(OnResumeButtonClicked);
            ExitButton.Button.onClick.AddListener(OnExitButtonClicked);

            ShootButton.OnPressed += OnShootButtonPressed;
            ShootButton.OnReleased += OnShootButtonReleased;

            ScoreItem.LabelText.text = Strings.STAT_SCORE;
            ScoreItem.ValueText.text = "0";
            
            WaveItem.LabelText.text = Strings.STAT_WAVE;
            WaveItem.ValueText.text = "0";
            
            LivesItem.LabelText.text = Strings.STAT_LIVES;
            LivesItem.ValueText.text = "0";
            
            var isMobilePlatform = AppController.IsMobilePlatform();
            ShootButton.gameObject.SetActive(isMobilePlatform);
            MoveJoystick.gameObject.SetActive(isMobilePlatform);
        }

        protected override void OnShow()
        {
            SetPauseOverlayVisible(false);
        }
        
        private void OnPauseButtonClicked()
        {
            GameController.PauseGame();
            AudioController.PlaySound(AudioController.ButtonPressSound);
        }
        
        private void OnResumeButtonClicked()
        {
            GameController.ResumeGame();
        }
        
        private void OnExitButtonClicked()
        {
            GameController.ExitGame();
        }

        private void OnShootButtonPressed()
        {
            InputController.SetInputAction(InputAction.Shoot, true);
        }
        
        private void OnShootButtonReleased()
        {
            InputController.SetInputAction(InputAction.Shoot, false);
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