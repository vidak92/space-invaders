using System.Collections.Generic;
using SGSTools.Components;
using UnityEngine;

namespace SpaceInvaders
{
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
        private GameController GameController => GameController.Instance;
        private GameplayConfig GameplayConfig => GameController.GameplayConfig;
        private ObjectPool<Enemy> _enemyPool;

        private Enemy[,] _enemies;
        private MoveState _moveState;
        private MoveDirection _moveDirection;
        private MoveDirection _lastHorizontalMoveDirection;
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

        private GameplayBounds GameplayBounds => GameplayConfig.GameplayBounds;
        private WaveConfig WaveConfig => GameplayConfig.WaveConfig;
        private UFOConfig UFOConfig => GameplayConfig.EnemiesConfig.UFOConfig;

        private float IdleDuration => WaveConfig.IdleDuration / _currentSpeedMultiplier;
        private float StepDuration => WaveConfig.StepDuration / _currentSpeedMultiplier;

        private int RowCount => WaveConfig.EnemyRows.Count;
        private int ColumnCount => WaveConfig.EnemiesPerRow;

        private float HorizontalCellSize => (GameplayBounds.Right - GameplayBounds.Left) / WaveConfig.MaxGridColumns;
        private float VerticalCellSize => (GameplayBounds.Top - GameplayBounds.Bottom) / WaveConfig.MaxGridRows;

        public Transform UFOTransform => _ufo.transform;

        public void Init()
        {
            _enemies = new Enemy[ColumnCount, RowCount];
            for (int r = 0; r < RowCount; r++)
            {
                var enemyType = WaveConfig.EnemyRows[r];
                for (int c = 0; c < ColumnCount; c++)
                {
                    var enemy = Instantiate(GameplayConfig.GameplayAssetsConfig.GetEnemyPrefab(enemyType)); // @TODO pool?
                    enemy.transform.parent = transform;
                    enemy.Init();
                    enemy.name = $"Enemy({r}, {c})";
                    enemy.gameObject.SetActive(false);
                    _enemies[c, r] = enemy;
                }
            }

            _ufo = Instantiate(GameplayConfig.GameplayAssetsConfig.UFO); // @TODO pool?
            _ufo.gameObject.SetActive(false);
        }

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
                    if (_enemies[c, r].gameObject.activeSelf)
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
                    _enemies[c, r].gameObject.SetActive(true);
                }
            }
            _currentGridIndex = new GridIndex(0, 0);
            _newGridIndex = _currentGridIndex;
            MoveToNewGridPosition(1f);
            
            _timer = 0f;
            _moveState = MoveState.Idle;
            _moveDirection = MoveDirection.Right;
            _lastHorizontalMoveDirection = _moveDirection;
            _currentSpeedMultiplier = WaveConfig.MinWaveSpeedMultiplier;

            UpdateShotDelay();
            _shotTimer = _shotDelay;

            _ufoTimer = 0f;
            UpdateUFOSpawnDuration();

            GameController.OnNewWave();
        }

        public void EndWave()
        {
            foreach (var enemy in _enemies)
            {
                enemy.gameObject.SetActive(false);
            }
            _ufo.gameObject.SetActive(false);
        }

        private bool IsCurrentWaveEmpty()
        {
            if (_ufo.gameObject.activeSelf)
            {
                return false;
            }

            foreach (var enemy in _enemies)
            {
                if (enemy.gameObject.activeSelf)
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
                        if (enemy.gameObject.activeSelf)
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
                    GameController.SpawnProjectile(GameplayConfig.EnemiesConfig.ProjectileConfig, shootingEnemy.transform.position);
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
                if (_moveDirection != MoveDirection.Down)
                {
                    _lastHorizontalMoveDirection = _moveDirection;
                }
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
                if (_lastHorizontalMoveDirection == MoveDirection.Left && canMoveRight) { return MoveDirection.Right; }
                if (_lastHorizontalMoveDirection == MoveDirection.Right && canMoveLeft) { return MoveDirection.Left; }
            }

            // @NOTE Fallback, shouldn't happen.
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

        private void UFOUpdate()
        {
            if (!UFOConfig.IsEnabled)
            {
                return;
            }
            if (_ufo.gameObject.activeSelf)
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