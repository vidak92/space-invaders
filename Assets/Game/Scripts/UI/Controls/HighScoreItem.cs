using SpaceInvaders.Common.Services;
using SpaceInvaders.Util;
using System;
using TMPro;
using UnityEngine;

namespace SpaceInvaders.UI.Controls
{
    public class HighScoreItem : MonoBehaviour
    {
        // Fields
        [SerializeField]
        private TMP_Text _placeText;

        [SerializeField]
        private TMP_Text _scoreText;

        [SerializeField]
        private TMP_Text _dateText;

        // Methods
        public void SetData(int place, HighScoreStats highScoreStats)
        {
            gameObject.SetActive(true);

            _placeText.text = $"{place}.";
            _scoreText.text = $"{DisplayStrings.ResultsScorePrefix}{highScoreStats.Score}";
            var date = DateTime.ParseExact(highScoreStats.Timestamp, Constants.TimestampFormat, null);
            _dateText.text = $"{date}";
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}