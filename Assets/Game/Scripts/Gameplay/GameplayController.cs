using MijanTools.Components;
using SpaceInvaders.Common;
using SpaceInvaders.Util;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        // Fields
        private GameManager _gameManager;
        private GameplayConfig _gameplayConfig;
        private GameplayAssetsConfig _gameplayAssetsConfig;
        private InputService _inputService;
        private GameStatsController _gameStatsController;
        private HighScoreService _highScoreService;
        private Camera _camera;

        private ObjectPool<Projectile> _projectilePool;
        private readonly int _projectilePoolInitialCapacity = 20;
        private Player _player;
        private EnemyWave _enemyWave;
        private float _resultsScreenTimer;

        [Inject]
        private void Init(GameManager gameManager,
            GameplayConfig gameplayConfig, 
            GameplayAssetsConfig gameplayAssetsConfig,
            InputService inputService, 
            GameStatsController gameStatsController, 
            HighScoreService highScoreService,
            Camera camera)
        {
            _gameManager = gameManager;
            _gameplayConfig = gameplayConfig;
            _gameplayAssetsConfig = gameplayAssetsConfig;
            _inputService = inputService;
            _gameStatsController = gameStatsController;
            _highScoreService = highScoreService;
            _camera = camera;

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
            var projectilePoolParent = new GameObject(Constants.ProjectilePoolObjectName).transform;
            projectilePoolParent.parent = transform;
            _projectilePool = new ObjectPool<Projectile>(_gameplayAssetsConfig.Projectile, _projectilePoolInitialCapacity, projectilePoolParent);

            _player = Instantiate(_gameplayAssetsConfig.Player, transform);
            _player.Init(_gameplayConfig, _projectilePool, _gameStatsController);
            _player.OnPlayerDied += OnPlayerDied;
            
            _enemyWave = Instantiate(_gameplayAssetsConfig.EnemyWave, transform);
            _enemyWave.Init(_gameplayAssetsConfig, _gameplayConfig, _projectilePool, _gameStatsController);
        }

        public void StartGame()
        {
            _player.SetActive(true);
            _player.ResetPosition();
            _player.ResetColor();

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
                    _gameManager.SetState(GameState.Results);
                }
            }
        }

        private void UpdateCameraSize()
        {
            _camera.orthographicSize = _gameplayConfig.CameraSize;
        }
    }
}