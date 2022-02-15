using SpaceInvaders.Common;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI
{
    public class ControlsScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private Button _exitButton;

        // Overrides
        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            _gameManager.SetState(GameState.MainMenu);
        }
    }
}