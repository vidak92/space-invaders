using System.Collections.Generic;
using SGSTools.Extensions;
using UnityEngine;

namespace SpaceInvaders
{
    public class HighScoresScreen : BaseScreen
    {
        public List<HighScoreItem> HighScoreItems = new List<HighScoreItem>();
        public GameObject PlaceholderView;
        public UIButton ExitButton;

        protected override void OnInit()
        {
            ExitButton.Text.text = Strings.BACK_BUTTON_TEXT;
            ExitButton.Button.onClick.AddListener(OnExitButtonClicked);

            PlaceholderView.gameObject.SetActive(false);
        }

        protected override void OnShow()
        {
            var highScoreList = HighScoreService.LoadHighScores();
            for (int i = 0; i < HighScoreItems.Count; i++)
            {
                if (highScoreList.Items.ContainsIndex(i))
                {
                    var highScoreStats = highScoreList.Items[i];
                    HighScoreItems[i].SetData(i + 1, highScoreStats);
                }
                else
                {
                    HighScoreItems[i].Hide();
                }
            }

            var isHighScoreListEmpty = highScoreList.Items.IsNullOrEmpty();
            PlaceholderView.gameObject.SetActive(isHighScoreListEmpty);
        }

        private void OnExitButtonClicked()
        {
            GameController.ShowMainMenu();
        }
    }
}