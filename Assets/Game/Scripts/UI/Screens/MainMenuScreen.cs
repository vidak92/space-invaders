using SpaceInvaders.Common;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI
{
    public class MainMenuScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private Button _playButton;
        
        [SerializeField]
        private Button _highScoresButton;

        // Overrides
        protected override void OnInit()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _highScoresButton.onClick.AddListener(OnHighScoresButtonClicked);
        }

        // Event Handlers
        private void OnPlayButtonClicked()
        {
            _gameManager.SetState(GameState.Gameplay);
        }

        private void OnHighScoresButtonClicked()
        {
            _gameManager.SetState(GameState.HighScores);
        }
    }
}