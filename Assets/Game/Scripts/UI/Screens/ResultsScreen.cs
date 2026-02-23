using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class ResultsScreen : BaseScreen
    {
        [SerializeField]
        private TMP_Text _scoreResultText;

        [SerializeField]
        private TMP_Text _waveResultText;

        [SerializeField]
        private Button _exitButton;

        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        // @TODO invoke
        public void SetStats(int score, int wave)
        {
            _scoreResultText.text = score.ToString();
            _waveResultText.text = wave.ToString();
        }

        private void OnExitButtonClicked()
        {
            GameController.SetState(GameState.MainMenu);
        }
    }
}