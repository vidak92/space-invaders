using SpaceInvaders.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI
{
    public class ResultsScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private TMP_Text _scoreResultText;

        [SerializeField]
        private TMP_Text _waveResultText;

        [SerializeField]
        private Button _exitButton;

        // Overrides
        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        protected override void OnShow()
        {
            var gameStats = _gameStatsController.CurrentStats;
            _scoreResultText.text = gameStats.Score.ToString();
            _waveResultText.text = gameStats.Wave.ToString();
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            _gameManager.SetState(GameState.MainMenu);
        }
    }
}