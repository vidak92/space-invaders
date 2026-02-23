using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public struct HighScoreStats
    {
        public int Score;
        public string Timestamp;

        public HighScoreStats(int score)
        {
            Score = score;
            Timestamp = GetTimestamp(DateTime.Now);
        }

        private static string GetTimestamp(DateTime value)
        {
            return value.ToString(Constants.TimestampFormat);
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
            highScoreList.Items.Sort((i1, i2) => i2.Score - i1.Score);
            while (highScoreList.Items.Count > _highScoreLimit)
            {
                highScoreList.Items.RemoveAt(highScoreList.Items.Count - 1);
            }
            SaveHighScores(highScoreList);
        }

        public HighScoreList LoadHighScores()
        {
            var highScoresJson = PlayerPrefs.GetString(PlayerPrefsKeys.HighScores, "");
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
                PlayerPrefs.SetString(PlayerPrefsKeys.HighScores, highScoresJson);
            }
            PlayerPrefs.Save();
        }
    }
}