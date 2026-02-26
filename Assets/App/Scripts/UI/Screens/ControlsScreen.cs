using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class ControlsScreen : BaseScreen
    {
        public TMP_Text TitleText;
        
        [Space]
        public TMP_Text ActionHeaderText;
        public TMP_Text PrimaryKeyHeaderText;
        public TMP_Text SecondaryKeyHeaderText;
        
        [Space]
        public TMP_Text MoveLeftActionText;
        public TMP_Text MoveRightActionText;
        public TMP_Text ShootActionText;
        
        [Space]
        public UIButton BackButton;

        protected override void OnInit()
        {
            TitleText.text = Strings.CONTROLS_TITLE;
            
            ActionHeaderText.text = Strings.CONTROLS_HEADER_ACTION;
            PrimaryKeyHeaderText.text = Strings.CONTROLS_HEADER_PRIMARY_KEY;
            SecondaryKeyHeaderText.text = Strings.CONTROLS_HEADER_SECONDARY_KEY;

            MoveLeftActionText.text = Strings.CONTROLS_ACTION_MOVE_LEFT;
            MoveRightActionText.text = Strings.CONTROLS_ACTION_MOVE_RIGHT;
            ShootActionText.text = Strings.CONTROLS_ACTION_SHOOT;
            
            BackButton.Text.text = Strings.BUTTON_BACK;
            BackButton.Button.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            GameController.ShowMainMenu();
        }
    }
}