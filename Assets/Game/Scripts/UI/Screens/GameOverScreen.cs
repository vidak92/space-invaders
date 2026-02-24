using TMPro;

namespace SpaceInvaders
{
    public class GameOverScreen : BaseScreen
    {
        public TMP_Text ScoreValueText;
        public TMP_Text WaveValueText;
        public UIButton ExitButton;

        protected override void OnInit()
        {
            ExitButton.Text.text = Strings.BACK_BUTTON_TEXT;
            ExitButton.Button.onClick.AddListener(OnExitButtonClicked);
        }

        // @TODO invoke
        public void SetStats(int score, int wave)
        {
            ScoreValueText.text = score.ToString();
            WaveValueText.text = wave.ToString();
        }

        private void OnExitButtonClicked()
        {
            GameController.ShowMainMenu();
        }
    }
}