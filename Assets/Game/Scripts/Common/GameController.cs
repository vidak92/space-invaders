using SGSTools.Components;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public enum GameState
    {
        MainMenu,
        Gameplay,
        Paused,
        GameOver,
        HighScores,
        Controls
    }

    public class GameController : MonoBehaviour
    {
        public GameConfig GameConfig;

        [Space]
        public EnemyFormation EnemyFormation;
        
        [Space]
        public ObjectShaker CameraShaker;
        public InputController InputController;
        public UIController UIController;
        

        public HighScoreService HighScoreService { get; private set; } = new HighScoreService();

        private ObjectPool<Projectile> _projectilePool;
        
        private GameState _currentState;
        private Player _player;
        
        private int _score;
        private int _wave;
        private int _lives;
        
        private void Awake()
        {
            ServiceLocator.Add<GameController>(this);
            
            Application.targetFrameRate = 60;
            Camera.main.orthographicSize = GameConfig.CameraSize;
            
            DebugDraw.IsEnabled = false;
            DebugDraw.Settings.DefaultColor = Color.gray;
            
            InputController.Init();
            UIController.Init();
            
            _projectilePool = ObjectPool<Projectile>.CreateWithGameObject(GameConfig.ProjectilePrefab, 100, "ProjectilePool");
            // _projectilePool.Parent.parent = transform;

            _player = Instantiate(GameConfig.PlayerPrefab);
            _player.OnPlayerKilled += GameOver;
            _player.Init();
            _player.transform.parent = transform;

            EnemyFormation.transform.parent = transform;
            EnemyFormation.Init();

            EnemyFormation.UFOTransform.parent = transform;

            ShowMainMenu();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DebugDraw.IsEnabled = !DebugDraw.IsEnabled;
            }
            
            switch (_currentState)
            {
                case GameState.MainMenu:
                {
                    break;
                }
                case GameState.Gameplay:
                {
                    InputController.UpdateActionsForStandalone();
                    if (_player.IsActive)
                    {
                        var playerInput = InputController.GetPlayerInput();
                        _player.OnUpdate(playerInput);
                        EnemyFormation.OnUpdate();
                    }
                    break;
                }
                case GameState.GameOver:
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

        public void ShowMainMenu()
        {
            Time.timeScale = 1f;
            UIController.StartScreenTransition(UIController.MainMenuScreen, onComplete: () =>
            {
                _currentState = GameState.MainMenu;
            });
        }

        public void ShowHighScores()
        {
            UIController.StartScreenTransition(UIController.HighScoresScreen, onComplete: () =>
            {
                _currentState = GameState.HighScores;
            });
        }
        
        public void ShowControls()
        {
            UIController.StartScreenTransition(UIController.ControlsScreen, onComplete: () =>
            {
                _currentState = GameState.Controls;
            });
        }

        public void StartGame()
        {
            Time.timeScale = 1f;
            UIController.StartScreenTransition(UIController.GameplayScreen, onComplete: () =>
            {
                _currentState = GameState.Gameplay;
                
                _score = 0;
                _wave = 0;
                _lives = GameConfig.PlayerConfig.StartLives;
            
                _player.ResetState();
                _player.SetActive(true);
                EnemyFormation.StartWave();
            });
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            _currentState = GameState.Paused;
            UIController.GameplayScreen.SetPauseOverlayVisible(true);
        }

        public void ResumeGame()
        {
            _currentState = GameState.Gameplay;
            Time.timeScale = 1f;
            UIController.GameplayScreen.SetPauseOverlayVisible(false);
        }

        public void GameOver()
        {
            UIController.StartScreenTransition(UIController.GameOverScreen, onComplete: () =>
            {
                _currentState = GameState.GameOver;
                _player.SetActive(false);
                EnemyFormation.EndWave();
                _projectilePool.ReturnAllActiveObjects();
                var highScoreStats = new HighScoreStats(_score);
                HighScoreService.AddNewHighScore(highScoreStats);
                UIController.GameOverScreen.SetStats(_score, _wave); 
            });
        }

        public void ExitGame()
        {
            EnemyFormation.EndWave();
            _player.SetActive(false);
            _projectilePool.ReturnAllActiveObjects();
            ShowMainMenu();
        }

        public void OnPlayerShot()
        {
            _lives--;
            _player.OnLifeLost(_lives);
            UIController.GameplayScreen.SetGameStats(_score, _wave, _lives);
            CameraShaker.StartShake(_lives > 0 ? 0.5f : 1f); // @TODO config?
            // @TODO update UI and game flow
        }

        public void OnEnemyShot(EnemyType enemyType)
        {
            _score += GameConfig.EnemiesConfig.GetScoreValueForEnemyType(enemyType);
            UIController.GameplayScreen.SetGameStats(_score, _wave, _lives);
            CameraShaker.StartShake(enemyType == EnemyType.UFO ? 0.5f : 0f); // @TODO config?
            // @TODO update UI and game flow
        }

        public void OnWaveStart()
        {
            _wave++;
            UIController.GameplayScreen.SetGameStats(_score, _wave, _lives);
            // @TODO update UI and game flow
        }

        public void SpawnProjectile(ProjectileConfig projectileConfig, Vector3 position)
        {
            var projectile = _projectilePool.Get();
            projectile.Init(GameConfig, projectileConfig, position);
        }
        
        public void DespawnProjectile(Projectile projectile)
        {
            _projectilePool.Return(projectile);
        }
    }
}