using MijanTools.Components;
using SpaceInvaders.Common;
using SpaceInvaders.Common.Services;
using SpaceInvaders.Common.State;
using SpaceInvaders.Gameplay.Config;
using SpaceInvaders.Gameplay.Objects;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        // Fields
        [Inject]
        private AppController _appController;
        [Inject]
        private GameplayConfig _gameplayConfig;
        [Inject]
        private GameplayAssetsConfig _gameplayAssetsConfig;

        [Inject]
        private InputService _inputService;
        [Inject]
        private HighScoreService _highScoreService;
        [Inject]
        private GameStatsController _gameStatsController;
        [Inject]
        private Camera _camera;

        [Inject]
        private Player.Factory _playerFactory;
        [Inject]
        private EnemyWave.Factory _enemyWaveFactory;
        [Inject]
        private ObjectPool<Projectile> _projectilePool;

        private Player _player;
        private EnemyWave _enemyWave;
        private float _resultsScreenTimer;

        [Inject]
        private void Init()
        {
            UpdateCameraSize();
            SetupPrefabs();
        }

        // Event Handlers
        private void OnPlayerDied()
        {
            var highScoreStats = new HighScoreStats(_gameStatsController.CurrentStats.Score);
            _highScoreService.AddNewHighScore(highScoreStats);
        }

        // Methods
        private void SetupPrefabs()
        {
            _projectilePool.Parent.parent = transform;

            _player = _playerFactory.Create(_gameplayAssetsConfig.Player);
            _player.OnPlayerDied += OnPlayerDied;
            _player.SetActive(false);
            _player.transform.parent = transform;

            _enemyWave = _enemyWaveFactory.Create(_gameplayAssetsConfig.EnemyWave);
            _enemyWave.transform.parent = transform;

            _enemyWave.UFOTransform.parent = transform;
        }

        public void StartGame()
        {
            _player.SetActive(true);
            _player.ResetState();

            _gameStatsController.ResetStats();

            _enemyWave.StartWave();

            _resultsScreenTimer = 0f;
        }

        public void ExitGame()
        {
            _player.SetActive(false);

            _enemyWave.EndWave();
        }

        public void OnUpdate()
        {
            if (_player.IsActive)
            {
                var playerInput = _inputService.GetPlayerInput();
                _player.OnUpdate(playerInput);

                _enemyWave.OnUpdate();
            }
            else
            {
                _resultsScreenTimer += Time.deltaTime;
                if (_resultsScreenTimer > _gameplayConfig.ResultsScreenDelay)
                {
                    _appController.SetState(GameState.Results);
                }
            }
        }

        private void UpdateCameraSize()
        {
            _camera.orthographicSize = _gameplayConfig.CameraSize;
        }
    }
}