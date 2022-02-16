using SpaceInvaders.Common.State;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI.Screens
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
            _appController.SetState(GameState.MainMenu);
        }
    }
}