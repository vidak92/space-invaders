using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
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

        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        protected override void UpdateControlsForCurrentPlatform()
        {
            // @TODO for mobile
            _moveLeftButton.gameObject.SetActive(false);
            _moveRightButton.gameObject.SetActive(false);
            _shootButton.gameObject.SetActive(false);
        }

        private void OnExitButtonClicked()
        {
            GameController.SetState(GameState.MainMenu);
        }

        // @TODO separate methods for each stat?
        public void SetGameStats(int score, int wave, int lives)
        {
            _scoreText.text = $"SCORE: {score}";
            _waveText.text = $"WAVE: {wave}";
            _livesText.text = $"LIVES: {lives}";
        }
    }
}