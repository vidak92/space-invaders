using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
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
            // @OTOD _appController.SetState(GameState.MainMenu);
        }
    }
}