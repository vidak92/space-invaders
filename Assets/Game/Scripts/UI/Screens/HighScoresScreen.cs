using MijanTools.Common;
using SpaceInvaders.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.UI
{
    public class HighScoresScreen : BaseScreen
    {
        // Fields
        [SerializeField]
        private List<HighScoreItem> _highScoreItems = new List<HighScoreItem>();

        [SerializeField]
        private GameObject _placeholderView;

        [SerializeField]
        private Button _exitButton;

        // Overrides
        protected override void OnInit()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);

            _placeholderView.gameObject.SetActive(false);
        }

        protected override void OnShow()
        {
            var highScoreList = _highScoreService.LoadHighScores();
            for (int i = 0; i < _highScoreItems.Count; i++)
            {
                if (highScoreList.Items.ContainsIndex(i))
                {
                    var highScoreStats = highScoreList.Items[i];
                    _highScoreItems[i].SetData(i + 1, highScoreStats);
                }
                else
                {
                    _highScoreItems[i].Hide();
                }
            }

            var isHighScoreListEmpty = highScoreList.Items.IsNullOrEmpty();
            _placeholderView.gameObject.SetActive(isHighScoreListEmpty);
        }

        // Event Handlers
        private void OnExitButtonClicked()
        {
            _gameManager.SetState(GameState.MainMenu);
        }
    }
}