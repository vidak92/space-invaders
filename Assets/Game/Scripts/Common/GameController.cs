using SGSTools.Components;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public enum GameState
    {
        Loading,
        MainMenu,
        Gameplay,
        Results,
        HighScores,
        Controls
    }

    public class GameController : MonoBehaviour
    {
        public GameConfig gameConfig;
        
        [Space]
        public InputController InputController;
        public UIController UIController;

        public HighScoreService HighScoreService { get; private set; } = new HighScoreService();

        private ObjectPool<Projectile> _projectilePool;
        
        private GameState _currentState;
        private Player _player;
        private EnemyWave _enemyWave;
        private float _resultsScreenTimer;
        
        private int _score;
        private int _wave;
        private int _lives;
        
        private void Awake()
        {
            ServiceLocator.Add<GameController>(this);
            
            Application.targetFrameRate = 60;
            Camera.main.orthographicSize = gameConfig.CameraSize;
            
            DebugDraw.IsEnabled = false;
            DebugDraw.Settings.DefaultColor = Color.gray;
            
            InputController.Init();
            UIController.Init();
            
            _projectilePool = ObjectPool<Projectile>.CreateWithGameObject(gameConfig.GameplayAssetsConfig.Projectile, 100, "ProjectilePool");
            // _projectilePool.Parent.parent = transform;

            _player = Instantiate(gameConfig.GameplayAssetsConfig.Player); // @TODO pool?
            _player.OnPlayerDied += OnPlayerDied;
            _player.Init();
            _player.gameObject.SetActive(false);
            _player.transform.parent = transform;

            _enemyWave = Instantiate(gameConfig.GameplayAssetsConfig.EnemyWave); // TODO pool?
            _enemyWave.transform.parent = transform;
            _enemyWave.Init();

            _enemyWave.UFOTransform.parent = transform;

            SetState(GameState.MainMenu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DebugDraw.IsEnabled = !DebugDraw.IsEnabled;
            }
            
            switch (_currentState)
            {
                case GameState.Loading:
                {
                    break;
                }
                case GameState.MainMenu:
                {
                    break;
                }
                case GameState.Gameplay:
                {
                    InputController.UpdateActionsForStandalone();
                    if (_player.gameObject.activeSelf)
                    {
                        var playerInput = InputController.GetPlayerInput();
                        _player.OnUpdate(playerInput);
                        _enemyWave.OnUpdate();
                    }
                    else
                    {
                        _resultsScreenTimer += Time.deltaTime;
                        if (_resultsScreenTimer > gameConfig.ResultsScreenDelay)
                        {
                            // @TODO show results
                        }
                    }
                    break;
                }
                case GameState.Results:
                {
                    break;
                }
                case GameState.HighScores:
                {
                    break;
                }
                case GameState.Controls:
                {
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public void SetState(GameState gameState)
        {
            _currentState = gameState;
            switch (_currentState)
            {
                case GameState.Loading:
                {
                    UIController.SetActiveScreen(UIController.LoadingScreen);
                    break;
                }
                case GameState.MainMenu:
                {
                    UIController.StartScreenTransition(UIController.MainMenuScreen, onComplete: () =>
                    {
                        ExitGame();
                    });
                    break;
                }
                case GameState.Gameplay:
                {
                    UIController.StartScreenTransition(UIController.GameplayScreen, onComplete: () =>
                    {
                        StartGame();
                    });
                    break;
                }
                case GameState.Results:
                {
                    UIController.StartScreenTransition(UIController.ResultsScreen, onComplete: () =>
                    {
                        _enemyWave.EndWave();
                        _projectilePool.ReturnAllActiveObjects();
                        var highScoreStats = new HighScoreStats(_score);
                        HighScoreService.AddNewHighScore(highScoreStats);
                        UIController.ResultsScreen.SetStats(_score, _wave); 
                    });
                    break;
                }
                case GameState.HighScores:
                {
                    UIController.StartScreenTransition(UIController.HighScoresScreen);
                    break;
                }
                case GameState.Controls:
                {
                    UIController.StartScreenTransition(UIController.ControlsScreen);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public void OnPlayerShot()
        {
            _lives--;
            _player.OnLifeLost(_lives);
            UIController.GameplayScreen.SetGameStats(_score, _wave, _lives);
            // @TODO update UI and game flow
        }

        public void OnEnemyShot(EnemyType enemyType)
        {
            _score += gameConfig.EnemiesConfig.GetScoreValueForEnemyType(enemyType);
            UIController.GameplayScreen.SetGameStats(_score, _wave, _lives);
            // @TODO update UI and game flow
        }

        public void OnNewWave()
        {
            _wave++;
            UIController.GameplayScreen.SetGameStats(_score, _wave, _lives);
            // @TODO update UI and game flow
        }
        
        private void OnPlayerDied()
        {
            SetState(GameState.Results);
        }

        public void StartGame()
        {
            _score = 0;
            _wave = 0;
            _lives = gameConfig.PlayerConfig.StartLives;
            
            _player.gameObject.SetActive(true);
            _player.ResetState();
            _enemyWave.StartWave();

            _resultsScreenTimer = 0f;
        }

        public void ExitGame()
        {
            _player.gameObject.SetActive(false);
            _enemyWave.EndWave();
        }

        public void SpawnProjectile(ProjectileConfig projectileConfig, Vector3 position)
        {
            var projectile = _projectilePool.Get();
            projectile.Init(gameConfig, projectileConfig, position);
        }
        
        public void DespawnProjectile(Projectile projectile)
        {
            _projectilePool.Return(projectile);
        }
    }
}