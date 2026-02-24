using System.Collections.Generic;
using SGSTools.Common;
using SGSTools.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders
{
    public enum FormationState
    {
        None,
        Spawn,
        Idle,
        Move,
        Empty,
    }

    public enum MoveDirection
    {
        Right,
        Down,
        Left
    }

    public class EnemyFormation : MonoBehaviour
    {
        private EnemyUFO _ufo;
        private Enemy[,] _enemies;
        
        private float _formationSpeedT;
        
        private FormationState _formationState;
        private Timer _formationTimer;
        private MoveDirection _moveDirection;
        private MoveDirection _lastHorizontalMoveDirection;
        private Vector2Int _currentGridIndex;
        private Vector2Int _nextGridIndex;

        private List<Enemy> _enemiesWhoCanShoot = new List<Enemy>();
        private FloatRange _shotDelayRange = new FloatRange();
        private Timer _shotTimer;

        private readonly Vector3 _startPosition = new Vector3(0f, 10f, 0f); // @TODO config

        private GameController GameController => ServiceLocator.Get<GameController>();
        private GameConfig GameConfig => GameController.GameConfig;
        private GameplayBounds GameplayBounds => GameConfig.GameplayBounds;
        private WaveConfig WaveConfig => GameConfig.WaveConfig;
        private UFOConfig UFOConfig => GameConfig.EnemiesConfig.UFOConfig;

        private float IdleDuration => WaveConfig.IdleDurationRange.GetValueAt(_formationSpeedT);
        private float StepDuration => WaveConfig.StepDurationRange.GetValueAt(_formationSpeedT);

        private int WaveRowCount => WaveConfig.EnemyRows.Count;
        private int WaveColCount => WaveConfig.EnemiesPerRow;

        private int GridRowCount => WaveConfig.GridSize.x;
        private int GridColCount => WaveConfig.GridSize.y;
        private float GridCellWidth => (GameplayBounds.Right - GameplayBounds.Left) / GridColCount;
        private float GridCellHeight => (GameplayBounds.Top - GameplayBounds.Bottom) / GridRowCount;

        public Transform UFOTransform => _ufo.transform;

        public void Init()
        {
            _enemies = new Enemy[WaveColCount, WaveRowCount];
            for (int r = 0; r < WaveRowCount; r++)
            {
                var enemyType = WaveConfig.EnemyRows[r];
                for (int c = 0; c < WaveColCount; c++)
                {
                    var enemy = Instantiate(GameConfig.GetEnemyPrefab(enemyType), transform);
                    enemy.Init($"{enemyType}_({c}, {r})");
                    _enemies[c, r] = enemy;
                }
            }

            _ufo = Instantiate(GameConfig.UFOPrefab);
            _ufo.Init();
            
            transform.position = _startPosition;

            SetFormationState(FormationState.None);
        }

        public void OnUpdate()
        {
            var dt = Time.deltaTime;
            
            // debug draw
            DebugDraw.Settings.SortLayerName = "Background";
            var bounds = GameConfig.GameplayBounds;
            var cellSize = new Vector3(GridCellWidth, GridCellHeight, 0f);
            var halfCellSize = cellSize / 2f;
            var topRight = new Vector3(bounds.Right + halfCellSize.x, bounds.Top + halfCellSize.y, 0f);
            var bottomRight = new Vector3(bounds.Right + halfCellSize.x, bounds.Bottom - halfCellSize.y, 0f);
            var bottomLeft = new Vector3(bounds.Left - halfCellSize.x, bounds.Bottom - halfCellSize.y, 0f);
            var topLeft = new Vector3(bounds.Left - halfCellSize.x, bounds.Top + halfCellSize.y, 0f);
            // DebugDraw.DrawLine(topRight, bottomRight, Color.red);
            // DebugDraw.DrawLine(bottomRight, bottomLeft, Color.red);
            // DebugDraw.DrawLine(bottomLeft, topLeft, Color.red);
            // DebugDraw.DrawLine(topLeft, topRight, Color.red);

            var gridWidth = bounds.Right - bounds.Left + GridCellWidth;
            var gridHeight = bounds.Top - bounds.Bottom + GridCellHeight;
            for (int r = 0; r <= GridRowCount + 1; r++)
            {
                var positionY = r * GridCellHeight;
                var position1 = bottomLeft + new Vector3(0f, positionY, 0f);
                var position2 = bottomLeft + new Vector3(gridWidth, positionY, 0f);
                DebugDraw.DrawLine(position1, position2);
            }
            for (int c = 0; c <= GridColCount + 1; c++)
            {
                var positionX = c * GridCellWidth;
                var position1 = bottomLeft + new Vector3(positionX, 0f, 0f);
                var position2 = bottomLeft + new Vector3(positionX, gridHeight, 0f);
                DebugDraw.DrawLine(position1, position2);
            }
            
            // wave speed
            var totalEnemies = _enemies.Length;
            var activeEnemies = 0;
            foreach (var enemy in _enemies)
            {
                if (enemy.IsActive)
                {
                    activeEnemies++;
                }
            }
            _formationSpeedT = 1f - (float)activeEnemies / totalEnemies;
            _formationSpeedT = Mathf.Pow(_formationSpeedT, WaveConfig.FormationSpeedTPower);

            switch (_formationState)
            {
                case FormationState.None:
                {
                    // @TODO ?
                    break;
                }
                case FormationState.Spawn:
                {
                    if (_formationTimer.Update(dt))
                    {
                        SetFormationState(FormationState.Idle);
                    }
                    break;
                }
                case FormationState.Idle:
                {
                    if (_formationTimer.Update(dt))
                    {
                        SetFormationState(FormationState.Move);
                    }
                    break;
                }
                case FormationState.Move:
                {
                    if (_formationTimer.Update(dt))
                    {
                        SetFormationState(FormationState.Idle);
                    }
                    else if (!_formationTimer.IsDone)
                    {
                        var t = 1f - Mathf.Clamp01(_formationTimer.Time / StepDuration);
                        LerpToNextGridPosition(t);
                    }
                    break;
                }
                case FormationState.Empty:
                {
                    // wait for new wave
                    if (_formationTimer.Update(dt))
                    {
                        StartWave();
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
            
            var isFormationActive = _formationState == FormationState.Idle || _formationState == FormationState.Move;
            if (isFormationActive)
            {
                // empty wave check
                if (IsCurrentWaveEmpty())
                {
                    SetFormationState(FormationState.Empty);
                }
            
                // shot
                if (_shotTimer.Update(dt))
                {
                    UpdateShotTimer();

                    _enemiesWhoCanShoot.Clear();
                    for (int c = 0; c < WaveColCount; c++)
                    {
                        for (int r = WaveRowCount - 1; r >= 0; r--)
                        {
                            var enemy = _enemies[c, r];
                            if (enemy.IsActive)
                            {
                                _enemiesWhoCanShoot.Add(enemy);
                                break;
                            }
                        }
                    }

                    if (_enemiesWhoCanShoot.Count > 0)
                    {
                        var shootingEnemyIndex = Random.Range(0, _enemiesWhoCanShoot.Count);
                        var shootingEnemy = _enemiesWhoCanShoot[shootingEnemyIndex];
                        GameController.SpawnProjectile(GameConfig.EnemiesConfig.ProjectileConfig, shootingEnemy.transform.position);
                    }
                }
            
                // ufo
                if (UFOConfig.IsEnabled)
                {
                    var ufoSpeed = UFOConfig.MoveSpeedRange.GetValueAt(_formationSpeedT);
                    _ufo.OnUpdate(ufoSpeed);
                }
            }
        }
        
        public void StartWave()
        {
            SetFormationState(FormationState.Spawn);
        }

        public void EndWave()
        {
            SetFormationState(FormationState.Empty);
        }

        private bool IsCurrentWaveEmpty()
        {
            if (_ufo.Enemy.IsActive)
            {
                return false;
            }

            foreach (var enemy in _enemies)
            {
                if (enemy.IsActive)
                {
                    return false;
                }
            }
            
            return true;
        }

        private void UpdateShotTimer()
        {
            _shotDelayRange.Min = WaveConfig.ShotDelayRangeMin.GetValueAt(_formationSpeedT);
            _shotDelayRange.Max = WaveConfig.ShotDelayRangeMax.GetValueAt(_formationSpeedT);
            var shotDelay = _shotDelayRange.GetRandomValue();
            _shotTimer.Reset(shotDelay);
        }

        private void SetFormationState(FormationState state)
        {
            _formationState = state;
            switch (_formationState)
            {
                case FormationState.None:
                {
                    _formationTimer.Init(0f);
                    break;
                }
                case FormationState.Spawn:
                {
                    _formationTimer.Reset(WaveConfig.SpawnDuration);
                    _moveDirection = MoveDirection.Right;
                    _lastHorizontalMoveDirection = _moveDirection;
                    _formationSpeedT = 1f;

                    var startGridIndex = new Vector2Int(0, 0);
                    var endGridIndex = new Vector2Int(WaveColCount - 1, WaveRowCount - 1);
                    var maxGridDistance = Vector2Int.Distance(startGridIndex, endGridIndex);
            
                    for (int r = 0; r < WaveRowCount; r++)
                    {
                        for (int c = 0; c < WaveColCount; c++)
                        {
                            var enemy = _enemies[c, r];
                            var spawnPosition = new Vector3(c * GridCellWidth, -r * GridCellHeight, 0f);
                            var spawnDuration = WaveConfig.SpawnDuration / 2f;
                            var gridDistance = Vector2Int.Distance(new Vector2Int(0, 0), new Vector2Int(c, r));
                            var spawnDelayT = gridDistance / maxGridDistance;
                            var spawnDelay = Mathf.Lerp(0f, spawnDuration / 2f, spawnDelayT);
                            enemy.Spawn(spawnPosition, spawnDelay, spawnDuration);
                        }
                    }
                    _currentGridIndex = new Vector2Int(0, 0);
                    _nextGridIndex = _currentGridIndex;
                    LerpToNextGridPosition(1f);

                    UpdateShotTimer();
                    
                    _ufo.OnWaveStart();
                    GameController.OnWaveStart();
                    break;
                }
                case FormationState.Idle:
                {
                    _formationTimer.Reset(IdleDuration);
                    _currentGridIndex = _nextGridIndex;
                    break;
                }
                case FormationState.Move:
                {
                    _formationTimer.Reset(StepDuration);
                    _moveDirection = GetNextMoveDirection(_moveDirection, _lastHorizontalMoveDirection);
                    if (_moveDirection != MoveDirection.Down)
                    {
                        _lastHorizontalMoveDirection = _moveDirection;
                    }
                    _nextGridIndex = GetTargetGridIndex(_currentGridIndex, _moveDirection);
                    break;
                }
                case FormationState.Empty:
                {
                    _formationTimer.Reset(WaveConfig.NewWaveDelay);
                    foreach (var enemy in _enemies)
                    {
                        enemy.SetActive(false);
                    }
                    _ufo.Enemy.SetActive(false);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        // grid
        private void LerpToNextGridPosition(float t)
        {
            var currentPosition = GetPositionForGridIndex(_currentGridIndex);
            var newPosition = GetPositionForGridIndex(_nextGridIndex);
            transform.position = Vector3.Lerp(currentPosition, newPosition, t);
        }

        private Vector3 GetPositionForGridIndex(Vector2Int gridIndex)
        {
            var startPosition = new Vector3(GameplayBounds.Left, GameplayBounds.Top, 0f);
            var offset = new Vector3(gridIndex.x * GridCellWidth, -gridIndex.y * GridCellHeight, 0f);
            return startPosition + offset;
        }

        private MoveDirection GetNextMoveDirection(MoveDirection currentMoveDirection, MoveDirection lastHorizontalMoveDirection)
        {
            var formationMinX = WaveColCount - 1;
            var formationMaxX = 0;
            var formationMinY = WaveRowCount - 1;
            var formationMaxY = 0;

            for (int r = 0; r < WaveRowCount; r++)
            {
                for (int c = 0; c < WaveColCount; c++)
                {
                    if (_enemies[c, r].IsActive)
                    {
                        formationMaxY = Mathf.Max(formationMaxY, r);
                        formationMinY = Mathf.Min(formationMinY, r);

                        formationMaxX = Mathf.Max(formationMaxX, c);
                        formationMinX = Mathf.Min(formationMinX, c);
                    }
                }
            }
            
            var canMoveRight = _currentGridIndex.x + formationMaxX < GridColCount;
            var canMoveLeft = _currentGridIndex.x + formationMinX > 0;
            var canMoveDown = _currentGridIndex.y + formationMaxY < GridRowCount;

            if (currentMoveDirection == MoveDirection.Right)
            {
                if (canMoveRight) { return MoveDirection.Right; }
                if (canMoveDown) { return MoveDirection.Down; }
                return MoveDirection.Left;

            }
            if (currentMoveDirection == MoveDirection.Left)
            {
                if (canMoveLeft) { return MoveDirection.Left; }
                if (canMoveDown) { return MoveDirection.Down; }
                return MoveDirection.Right;
            }
            if (currentMoveDirection == MoveDirection.Down)
            {
                if (lastHorizontalMoveDirection == MoveDirection.Left && canMoveRight) { return MoveDirection.Right; }
                if (lastHorizontalMoveDirection == MoveDirection.Right && canMoveLeft) { return MoveDirection.Left; }
            }

            // @NOTE Fallback, shouldn't happen.
            return currentMoveDirection;
        }

        private Vector2Int GetTargetGridIndex(Vector2Int currentGridIndex, MoveDirection moveDirection)
        {
            var targetGridIndex = currentGridIndex;
            if (_moveDirection == MoveDirection.Right) { targetGridIndex.x++; }
            if (_moveDirection == MoveDirection.Left) { targetGridIndex.x--; }
            if (_moveDirection == MoveDirection.Down) { targetGridIndex.y++; }
            return targetGridIndex;
        }
    }
}