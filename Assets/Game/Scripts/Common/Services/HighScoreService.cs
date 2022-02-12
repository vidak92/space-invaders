using SpaceInvaders.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Common
{
    // Structs
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
        // Fields
        private readonly int _highScoreLimit = 10;

        // Methods
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
            var highScoreListlist = JsonUtility.FromJson<HighScoreList>(highScoresJson);
            if (highScoreListlist == null)
            {
                highScoreListlist = new HighScoreList();
            }
            return highScoreListlist;
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