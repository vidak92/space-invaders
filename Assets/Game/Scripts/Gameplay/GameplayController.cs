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
        private AppController _appController;
        private GameplayConfig _gameplayConfig;
        private GameplayAssetsConfig _gameplayAssetsConfig;

        private InputService _inputService;
        private HighScoreService _highScoreService;
        private GameStatsController _gameStatsController;
        private Camera _camera;
        
        private Player.Factory _playerFactory;
        private EnemyWave.Factory _enemyWaveFactory;
        private ObjectPool<Projectile> _projectilePool;
        
        private Player _player;
        private EnemyWave _enemyWave;
        private float _resultsScreenTimer;

        [Inject]
        private void Init(AppController appController,
            GameplayConfig gameplayConfig, 
            GameplayAssetsConfig gameplayAssetsConfig,
            InputService inputService, 
            HighScoreService highScoreService,
            GameStatsController gameStatsController,
            Camera camera,
            ObjectPool<Projectile> projectilePool,
            Player.Factory playerFactory,
            EnemyWave.Factory enemyWaveFactory)
        {
            _appController = appController;
            _gameplayConfig = gameplayConfig;
            _gameplayAssetsConfig = gameplayAssetsConfig;
            _inputService = inputService;
            _highScoreService = highScoreService;
            _gameStatsController = gameStatsController;
            _camera = camera;
            _projectilePool = projectilePool;
            _playerFactory = playerFactory;
            _enemyWaveFactory = enemyWaveFactory;

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