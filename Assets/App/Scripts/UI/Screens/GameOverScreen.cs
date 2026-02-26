using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class GameOverScreen : BaseScreen
    {
        public TMP_Text TitleText;
        
        [Space]
        public TMP_Text ScoreLabelText;
        public TMP_Text ScoreValueText;
        public TMP_Text WaveLabelText;
        public TMP_Text WaveValueText;
        
        [Space]
        public UIButton BackButton;

        protected override void OnInit()
        {
            TitleText.text = Strings.GAME_OVER_TITLE;
            ScoreLabelText.text = Strings.GAME_OVER_SCORE_PREFIX;
            WaveLabelText.text = Strings.GAME_OVER_WAVE_PREFIX;
            
            BackButton.Text.text = Strings.BUTTON_BACK;
            BackButton.Button.onClick.AddListener(OnExitButtonClicked);
        }

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