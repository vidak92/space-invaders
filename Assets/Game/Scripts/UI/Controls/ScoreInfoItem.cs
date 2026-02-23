using TMPro;
using UnityEngine;

namespace SpaceInvaders
{
    public class ScoreInfoItem : MonoBehaviour
    {
        // Fields
        [SerializeField]
        private EnemyType _enemyType;

        [SerializeField]
        private TMP_Text _scoreInfoText;

        // Properties
        public EnemyType EnemyType => _enemyType;

        // Methods
        public void SetPointsValue(int points)
        {
            _scoreInfoText.text = $"{DisplayStrings.ScoreInfoPrefix}{points}{DisplayStrings.ScoreInfoSuffix}";
        }

        public void SetMysteryValue()
        {
            _scoreInfoText.text = $"{DisplayStrings.ScoreInfoMystery}";
        }
    }
}