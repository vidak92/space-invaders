using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public struct HighScoreStats
    {
        public int Score;
        public int Wave;
        public string Timestamp;

        public HighScoreStats(int score, int wave)
        {
            Score = score;
            Wave = wave;
            Timestamp = DateTime.Now.ToString(Constants.TIMESTAMP_FORMAT);
        }
    }

    public class HighScoreList
    {
        public List<HighScoreStats> Items = new List<HighScoreStats>();
    }

    public class HighScoreService
    {
        private readonly int _highScoreLimit = 10;

        public void AddNewHighScore(HighScoreStats highScoreStats)
        {
            var highScoreList = LoadHighScores();
            highScoreList.Items.Add(highScoreStats);

            var sortedItems = highScoreList.Items
                .OrderByDescending(i => i.Score)
                .ThenByDescending(i => i.Wave)
                .ThenBy(i => i.Timestamp)
                .ToList();
            
            if (sortedItems.Count > _highScoreLimit)
            {
                sortedItems.RemoveRange(_highScoreLimit, sortedItems.Count - _highScoreLimit);
            }
            
            highScoreList.Items = sortedItems;
            SaveHighScores(highScoreList);
        }

        public HighScoreList LoadHighScores()
        {
            var highScoresJson = PlayerPrefs.GetString(PlayerPrefsKeys.HIGH_SCORES, "");
            var highScoreList = JsonUtility.FromJson<HighScoreList>(highScoresJson);
            if (highScoreList == null)
            {
                highScoreList = new HighScoreList();
            }
            return highScoreList;
        }

        private void SaveHighScores(HighScoreList highScoreList)
        {
            var highScoresJson = JsonUtility.ToJson(highScoreList);
            if (!string.IsNullOrEmpty(highScoresJson))
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.HIGH_SCORES, highScoresJson);
            }
            PlayerPrefs.Save();
        }
    }
}