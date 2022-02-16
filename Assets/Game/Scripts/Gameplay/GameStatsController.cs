using SpaceInvaders.Gameplay.Config;
using SpaceInvaders.Gameplay.Objects;
using System;

namespace SpaceInvaders.Gameplay
{
    // Structs
    public struct GameStats
    {
        public int Score;
        public int Wave;
        public int Lives;

        public GameStats(int score, int wave, int lives)
        {
            Score = score;
            Wave = wave;
            Lives = lives;
        }
    }

    public class GameStatsController
    {
        // Fields
        private GameplayConfig _gameplayConfig;

        private GameStats _gameStats;

        // Properties
        public Action<GameStats> OnGameStatsUpdated { get; set; }
        public Action<int> OnLifeLost { get; set; }

        public GameStats CurrentStats => _gameStats;

        public GameStatsController(GameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;

            ResetStats();
        }

        // Methods
        public void OnPlayerShot()
        {
            _gameStats.Lives--;
            OnGameStatsUpdated?.Invoke(_gameStats);
            OnLifeLost?.Invoke(_gameStats.Lives);
        }

        public void OnEnemyShot(EnemyType enemyType)
        {
            _gameStats.Score += _gameplayConfig.EnemiesConfig.GetScoreValueForEnemyType(enemyType);
            OnGameStatsUpdated?.Invoke(_gameStats);
        }

        public void OnNewWave()
        {
            _gameStats.Wave++;
            OnGameStatsUpdated?.Invoke(_gameStats);
        }

        public void ResetStats()
        {
            _gameStats = new GameStats(0, 0, _gameplayConfig.PlayerConfig.StartLives);
        }
    }
}