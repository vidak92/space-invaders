using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class ScoreInfoItem : MonoBehaviour
    {
        public EnemyType EnemyType;
        public TMP_Text ScoreInfoText;

        public void SetPointsValue(int points)
        {
            ScoreInfoText.text = $"{Strings.SCORE_INFO_PREFIX}{points}{Strings.SCORE_INFO_SUFFIX}";
        }

        public void SetMysteryValue()
        {
            ScoreInfoText.text = $"{Strings.SCORE_INFO_MYSTERY}";
        }
    }
}