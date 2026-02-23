using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class ControlsScreen : BaseScreen
    {
        [SerializeField]
        private Button _exitButton;

        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            GameController.SetState(GameState.MainMenu);
        }
    }
}