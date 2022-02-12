using SpaceInvaders.Common;
using SpaceInvaders.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    // Structs
    public enum MoveState
    {
        Idle,
        Moving
    }

    public enum MoveDirection
    {
        Right,
        Down,
        Left
    }

    public struct GridIndex
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public GridIndex(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public struct GridBounds
    {
        public int MinRow { get; set; }
        public int MaxRow { get; set; }
        public int MinColumn { get; set; }
        public int MaxColumn { get; set; }

        public GridBounds(int minRow, int maxRow, int minColumn, int maxColumn)
        {
            MinRow = minRow;
            MaxRow = maxRow;
            MinColumn = minColumn;
            MaxColumn = maxColumn;
        }
    }

    public class EnemyWave : MonoBehaviour
    {
        // Fields
        private GameplayAssetsConfig _gameplayAssetsConfig;
        private GameplayConfig _gameplayConfig;
        private ObjectPool<Projectile> _projectilePool;
        private GameStatsController _gameStatsController;

        private Enemy[,] _enemies;
        private MoveState _moveState;
        private MoveDirection _moveDirection;
        private GridIndex _currentGridIndex;
        private GridIndex _newGridIndex;
        private float _timer;
        private float _currentSpeedMultiplier;

        private List<Enemy> _enemiesWhoCanShoot = new List<Enemy>();
        private float _shotTimer;
        private float _shotDelay;

        private EnemyUFO _ufo;
        private float _ufoTimer;
        private float _ufoSpawnDuration;

        // Properties
        private GameplayBounds GameplayBounds => _gameplayConfig.GameplayBounds;
        private WaveConfig WaveConfig => _gameplayConfig.WaveConfig;
        private UFOConfig UFOConfig => _gameplayConfig.EnemiesConfig.UFOConfig;

        private float IdleDuration => WaveConfig.IdleDuration / _currentSpeedMultiplier;
        private float StepDuration => WaveConfig.StepDuration / _currentSpeedMultiplier;

        private int RowCount => WaveConfig.EnemyRows.Count;
        private int ColumnCount => WaveConfig.EnemiesPerRow;

        private float HorizontalCellSize => (GameplayBounds.Right - GameplayBounds.Left) / WaveConfig.MaxGridColumns;
        private float VerticalCellSize => (GameplayBounds.Top - GameplayBounds.Bottom) / WaveConfig.MaxGridRows;

        public void Init(GameplayAssetsConfig gameplayAssetsConfig, 
            GameplayConfig gameplayConfig, 
            ObjectPool<Projectile> projectilePool, 
            GameStatsController gameStatsController)
        {
            _gameplayAssetsConfig = gameplayAssetsConfig;
            _gameplayConfig = gameplayConfig;
            _projectilePool = projectilePool;
            _gameStatsController = gameStatsController;

            _enemies = new Enemy[ColumnCount, RowCount];
            for (int r = 0; r < RowCount; r++)
            {
                var enemyType = WaveConfig.EnemyRows[r];
                for (int c = 0; c < ColumnCount; c++)
                {
                    var enemy = Instantiate(_gameplayAssetsConfig.GetEnemyPrefab(enemyType), transform);
                    enemy.name = $"Enemy({r}, {c})";
                    enemy.Init(_gameplayConfig, _gameStatsController);
                    _enemies[c, r] = enemy;
                }
            }

            _ufo = Instantiate(_gameplayAssetsConfig.UFO);
            _ufo.Init(_gameplayConfig, gameStatsController);
        }

        // Methods
        public void OnUpdate()
        {
            UpdateSpeedMultiplier();

            if (IsCurrentWaveEmpty())
            {
                WaitForNewWaveUpdate();
            }
            else
            {
                if (_moveState == MoveState.Idle)
                {
                    IdleUpdate();
                }
                else if (_moveState == MoveState.Moving)
                {
                    MoveUpdate();
                }
            }
            ShotUpdate();
            UFOUpdate();
        }

        private void UpdateSpeedMultiplier()
        {
            var totalEnemies = _enemies.Length;
            var activeEnemies = 0;
            foreach (var enemy in _enemies)
            {
                if (enemy.gameObject.activeSelf) { activeEnemies++; }
            }
            var t = 1f - (float)activeEnemies / totalEnemies;
            _currentSpeedMultiplier = Mathf.Lerp(WaveConfig.MinWaveSpeedMultiplier, WaveConfig.MaxWaveSpeedMultiplier, Mathf.Pow(t, 4f));
        }

        private GridBounds GetCurrentGridBounds()
        {
            var minRow = RowCount - 1;
            var maxRow = 0;
            var minColumn = ColumnCount - 1;
            var maxColumn = 0;

            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    if (_enemies[c, r].IsActive)
                    {
                        maxRow = Mathf.Max(maxRow, r);
                        minRow = Mathf.Min(minRow, r);

                        maxColumn = Mathf.Max(maxColumn, c);
                        minColumn = Mathf.Min(minColumn, c);
                    }
                }
            }

            var bounds = new GridBounds(minRow, maxRow, minColumn, maxColumn);
            return bounds;
        }

        // Wave Methods
        public void StartWave()
        {
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColumnCount; c++)
                {
                    _enemies[c, r].transform.localPosition = new Vector3(c * HorizontalCellSize, 0f, -r * VerticalCellSize);
                    _enemies[c, r].SetAcitve(true);
                }
            }
            _currentGridIndex = new GridIndex(0, 0);
            _newGridIndex = _currentGridIndex;
            MoveToNewGridPosition(1f);
            
            _timer = 0f;
            _moveState = MoveState.Idle;
            _moveDirection = MoveDirection.Right;
            _currentSpeedMultiplier = WaveConfig.MinWaveSpeedMultiplier;

            UpdateShotDelay();
            _shotTimer = _shotDelay;

            _ufoTimer = 0f;
            UpdateUFOSpawnDuration();

            _gameStatsController.OnNewWave();
        }

        public void EndWave()
        {
            foreach (var enemy in _enemies)
            {
                enemy.SetAcitve(false);
            }
            _ufo.SetAcitve(false);
        }

        private bool IsCurrentWaveEmpty()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.IsActive)
                {
                    return false;
                }
            }
            return true;
        }

        private void WaitForNewWaveUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer >= WaveConfig.NewWaveDelay)
            {
                StartWave();
            }
        }

        // Shoot Methods
        private void UpdateShotDelay()
        {
            _shotDelay = Random.Range(WaveConfig.MinShotDelay, WaveConfig.MaxShotDelay) / _currentSpeedMultiplier;
        }

        private void ShotUpdate()
        {
            _shotTimer -= Time.deltaTime;
            if (_shotTimer <= 0f)
            {
                UpdateShotDelay();
                _shotTimer = _shotDelay;

                _enemiesWhoCanShoot.Clear();
                for (int c = 0; c < ColumnCount; c++)
                {
                    for (int r = RowCount - 1; r >= 0; r--)
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
                    var projectile = _projectilePool.Get();
                    projectile.Init(_gameplayConfig, _gameplayConfig.EnemiesConfig.ProjectileConfig, shootingEnemy.transform.position);
                }
            }
        }

        // Move Methods
        private void IdleUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer > IdleDuration)
            {
                _timer -= IdleDuration;
                _moveState = MoveState.Moving;
                _moveDirection = GetNextMoveDirection();
                _newGridIndex = GetTargetGridIndex();
            }
        }

        private void MoveUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer > StepDuration)
            {
                _timer -= StepDuration;
                _currentGridIndex = _newGridIndex;
                _moveState = MoveState.Idle;
            }
            else
            {
                MoveToNewGridPosition(_timer);
            }
        }

        private void MoveToNewGridPosition(float timer)
        {
            var currentPosition = GetPositionForGridIndex(_currentGridIndex);
            var newPosition = GetPositionForGridIndex(_newGridIndex);
            var t = Mathf.Clamp01(timer / StepDuration);
            transform.position = Vector3.Lerp(currentPosition, newPosition, t);
        }

        private Vector3 GetPositionForGridIndex(GridIndex gridIndex)
        {
            var startPosition = new Vector3(GameplayBounds.Left, 0f, GameplayBounds.Top);
            var offset = new Vector3(gridIndex.Column * HorizontalCellSize, 0f, -gridIndex.Row * VerticalCellSize);
            return startPosition + offset;
        }

        private MoveDirection GetNextMoveDirection()
        {
            var currentGridBounds = GetCurrentGridBounds();
            var canMoveRight = _currentGridIndex.Column + currentGridBounds.MaxColumn < WaveConfig.MaxGridColumns;
            var canMoveLeft = _currentGridIndex.Column + currentGridBounds.MinColumn > 0;
            var canMoveDown = _currentGridIndex.Row + currentGridBounds.MaxRow < WaveConfig.MaxGridRows;

            if (_moveDirection == MoveDirection.Right)
            {
                if (canMoveRight) { return MoveDirection.Right; }
                if (canMoveDown) { return MoveDirection.Down; }
                return MoveDirection.Left;

            }
            if (_moveDirection == MoveDirection.Left)
            {
                if (canMoveLeft) { return MoveDirection.Left; }
                if (canMoveDown) { return MoveDirection.Down; }
                return MoveDirection.Right;
            }
            if (_moveDirection == MoveDirection.Down)
            {
                if (canMoveRight) { return MoveDirection.Right; }
                if (canMoveLeft) { return MoveDirection.Left; }
            }

            // Fallback, shouldn't happen.
            return _moveDirection;
        }

        private GridIndex GetTargetGridIndex()
        {
            var newGridIndex = _currentGridIndex;
            if (_moveDirection == MoveDirection.Right) { newGridIndex.Column++; }
            if (_moveDirection == MoveDirection.Left) { newGridIndex.Column--; }
            if (_moveDirection == MoveDirection.Down) { newGridIndex.Row++; }
            return newGridIndex;
        }

        // UFO Methods
        private void UFOUpdate()
        {
            if (!UFOConfig.IsEnabled)
            {
                return;
            }
            if (_ufo.IsActive)
            {
                _ufo.OnUpdate(_currentSpeedMultiplier);
            }
            else
            {
                if (_currentGridIndex.Row > 0)
                {
                    _ufoTimer += Time.deltaTime;
                    if (_ufoTimer > _ufoSpawnDuration)
                    {
                        _ufoTimer -= _ufoSpawnDuration;
                        UpdateUFOSpawnDuration();
                        _ufo.Spawn();
                    }
                }
            }
        }

        private void UpdateUFOSpawnDuration()
        {
            _ufoSpawnDuration = Random.Range(UFOConfig.MinSpawnDuration, UFOConfig.MaxSpawnDuration) / _currentSpeedMultiplier;
        }
    }
}