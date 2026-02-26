using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class HighScoreItem : MonoBehaviour
    {
        public TMP_Text RankText;
        public TMP_Text ScoreText;
        public TMP_Text WaveText;

        public void SetData(int place, HighScoreStats highScoreStats)
        {
            gameObject.SetActive(true);

            RankText.text = $"{place}.";
            ScoreText.text = $"{highScoreStats.Score}";
            WaveText.text = $"{highScoreStats.Wave}";
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}