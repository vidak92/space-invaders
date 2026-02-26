using System.Collections.Generic;
using SGSTools.Extensions;
using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class HighScoresScreen : BaseScreen
    {
        public TMP_Text TitleText;
        
        [Space]
        public HighScoreItem HeaderItem;
        public List<HighScoreItem> HighScoreItems = new List<HighScoreItem>();
        public GameObject PlaceholderView;
        
        [Space]
        public UIButton BackButton;

        protected override void OnInit()
        {
            TitleText.text = Strings.HIGH_SCORES_TITLE;
            
            HeaderItem.RankText.text = Strings.HIGH_SCORES_HEADER_RANK;
            HeaderItem.ScoreText.text = Strings.HIGH_SCORES_HEADER_SCORE;
            HeaderItem.WaveText.text = Strings.HIGH_SCORES_HEADER_WAVE;
            
            BackButton.Text.text = Strings.BUTTON_BACK;
            BackButton.Button.onClick.AddListener(OnExitButtonClicked);

            PlaceholderView.gameObject.SetActive(false);
        }

        protected override void OnShow()
        {
            var highScoreList = AppController.HighScoreService.LoadHighScores();
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