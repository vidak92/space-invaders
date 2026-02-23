using SGSTools.Components;
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
        public static GameController Instance { get; private set; }

        public GameplayConfig GameplayConfig;
        
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
            Instance = this;
            
            Application.targetFrameRate = 60;
            Camera.main.orthographicSize = GameplayConfig.CameraSize;
            
            InputController.Init();
            UIController.Init();
            
            _projectilePool = ObjectPool<Projectile>.CreateWithGameObject(GameplayConfig.GameplayAssetsConfig.Projectile, 100, "ProjectilePool");
            // _projectilePool.Parent.parent = transform;

            _player = Instantiate(GameplayConfig.GameplayAssetsConfig.Player); // @TODO pool?
            _player.OnPlayerDied += OnPlayerDied;
            _player.Init();
            _player.gameObject.SetActive(false);
            _player.transform.parent = transform;

            _enemyWave = Instantiate(GameplayConfig.GameplayAssetsConfig.EnemyWave); // TODO pool?
            _enemyWave.transform.parent = transform;
            _enemyWave.Init();

            _enemyWave.UFOTransform.parent = transform;

            SetState(GameState.MainMenu);
        }

        private void Update()
        {
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
                        if (_resultsScreenTimer > GameplayConfig.ResultsScreenDelay)
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
                    ExitGame();
                    UIController.SetActiveScreen(UIController.MainMenuScreen);
                    break;
                }
                case GameState.Gameplay:
                {
                    StartGame();
                    UIController.SetActiveScreen(UIController.GameplayScreen);
                    break;
                }
                case GameState.Results:
                {
                    _enemyWave.EndWave();
                    var highScoreStats = new HighScoreStats(_score);
                    HighScoreService.AddNewHighScore(highScoreStats);
                    UIController.SetActiveScreen(UIController.ResultsScreen);
                    UIController.ResultsScreen.SetStats(_score, _wave);
                    break;
                }
                case GameState.HighScores:
                {
                    UIController.SetActiveScreen(UIController.HighScoresScreen);
                    break;
                }
                case GameState.Controls:
                {
                    UIController.SetActiveScreen(UIController.ControlsScreen);
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
            _score += GameplayConfig.EnemiesConfig.GetScoreValueForEnemyType(enemyType);
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
            _lives = GameplayConfig.PlayerConfig.StartLives;
            
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
            projectile.Init(GameplayConfig, projectileConfig, position);
        }
        
        public void DespawnProjectile(Projectile projectile)
        {
            _projectilePool.Return(projectile);
        }
    }
}