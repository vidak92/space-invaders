using System.Collections.Generic;
using SGSTools.Util;
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

        private readonly Vector3 _startPosition = new Vector3(0f, 10f, 0f); // @TODO config

        private GameController GameController => ServiceLocator.Get<GameController>();
        private GameConfig GameConfig => GameController.gameConfig;
        private GameplayBounds GameplayBounds => GameConfig.GameplayBounds;
        private WaveConfig WaveConfig => GameConfig.WaveConfig;
        private UFOConfig UFOConfig => GameConfig.EnemiesConfig.UFOConfig;

        private float IdleDuration => WaveConfig.IdleDuration / _currentSpeedMultiplier;
        private float StepDuration => WaveConfig.StepDuration / _currentSpeedMultiplier;

        private int RowCount => WaveConfig.EnemyRows.Count;
        private int ColumnCount => WaveConfig.EnemiesPerRow;

        private int GridRowCount => WaveConfig.GridSize.x;
        private int GridColCount => WaveConfig.GridSize.y;
        private float CellWidth => (GameplayBounds.Right - GameplayBounds.Left) / GridColCount;
        private float CellHeight => (GameplayBounds.Top - GameplayBounds.Bottom) / GridRowCount;

        public Transform UFOTransform => _ufo.transform;

        public void Init()
        {
            _enemies = new Enemy[ColumnCount, RowCount];
            for (int r = 0; r < RowCount; r++)
            {
                var enemyType = WaveConfig.EnemyRows[r];
                for (int c = 0; c < ColumnCount; c++)
                {
                    var enemy = Instantiate(GameConfig.GameplayAssetsConfig.GetEnemyPrefab(enemyType), transform);
                    enemy.Init($"{enemyType}_({r}, {c})");
                    _enemies[c, r] = enemy;
                }
            }

            _ufo = Instantiate(GameConfig.GameplayAssetsConfig.UFO);
            _ufo.Init($"{EnemyType.UFO}");
            
            transform.position = _startPosition;
        }

        public void OnUpdate()
        {
            DebugDraw.Settings.SortLayerName = "Background";
            var bounds = GameConfig.GameplayBounds;
            var cellSize = new Vector3(CellWidth, CellHeight, 0f);
            var halfCellSize = cellSize / 2f;
            var topRight = new Vector3(bounds.Right + halfCellSize.x, bounds.Top + halfCellSize.y, 0f);
            var bottomRight = new Vector3(bounds.Right + halfCellSize.x, bounds.Bottom - halfCellSize.y, 0f);
            var bottomLeft = new Vector3(bounds.Left - halfCellSize.x, bounds.Bottom - halfCellSize.y, 0f);
            var topLeft = new Vector3(bounds.Left - halfCellSize.x, bounds.Top + halfCellSize.y, 0f);
            // DebugDraw.DrawLine(topRight, bottomRight, Color.red);
            // DebugDraw.DrawLine(bottomRight, bottomLeft, Color.red);
            // DebugDraw.DrawLine(bottomLeft, topLeft, Color.red);
            // DebugDraw.DrawLine(topLeft, topRight, Color.red);

            var gridWidth = bounds.Right - bounds.Left + CellWidth;
            var gridHeight = bounds.Top - bounds.Bottom + CellHeight;
            for (int r = 0; r <= GridRowCount + 1; r++)
            {
                var positionY = r * CellHeight;
                var position1 = bottomLeft + new Vector3(0f, positionY, 0f);
                var position2 = bottomLeft + new Vector3(gridWidth, positionY, 0f);
                DebugDraw.DrawLine(position1, position2);
            }
            for (int c = 0; c <= GridColCount + 1; c++)
            {
                var positionX = c * CellWidth;
                var position1 = bottomLeft + new Vector3(positionX, 0f, 0f);
                var position2 = bottomLeft + new Vector3(positionX, gridHeight, 0f);
                DebugDraw.DrawLine(position1, position2);
            }
            // var center = new Vector3((bounds.Right + bounds.Left) / 2f, (bounds.Top + bounds.Bottom) / 2f, 0f);
            // var size = new Vector2(bounds.Right - bounds.Left, bounds.Top - bounds.Bottom);
            // DebugDraw.DrawRect(center, size);
            
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
            _currentSpeedMultiplier = WaveConfig.WaveSpeedMultiplierRange.GetValueAt(Mathf.Pow(t, 4f));
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
                    _enemies[c, r].transform.localPosition = new Vector3(c * CellWidth, -r * CellHeight, 0f);
                    _enemies[c, r].gameObject.SetActive(true);
                }
            }
            _currentGridIndex = new GridIndex(0, 0);
            _newGridIndex = _currentGridIndex;
            MoveToNextGridPosition(1f);
            
            _timer = 0f;
            _moveState = MoveState.Idle;
            _moveDirection = MoveDirection.Right;
            _lastHorizontalMoveDirection = _moveDirection;
            _currentSpeedMultiplier = WaveConfig.WaveSpeedMultiplierRange.Min;

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
            // @TODO should this be randomized?
            _shotDelay = WaveConfig.ShotDelayRange.GetRandomValue() / _currentSpeedMultiplier;
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
                    GameController.SpawnProjectile(GameConfig.EnemiesConfig.ProjectileConfig, shootingEnemy.transform.position);
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
                MoveToNextGridPosition(_timer);
            }
        }

        private void MoveToNextGridPosition(float timer)
        {
            var currentPosition = GetPositionForGridIndex(_currentGridIndex);
            var newPosition = GetPositionForGridIndex(_newGridIndex);
            var t = Mathf.Clamp01(timer / StepDuration);
            transform.position = Vector3.Lerp(currentPosition, newPosition, t);
        }

        private Vector3 GetPositionForGridIndex(GridIndex gridIndex)
        {
            var startPosition = new Vector3(GameplayBounds.Left, GameplayBounds.Top, 0f);
            var offset = new Vector3(gridIndex.Column * CellWidth, -gridIndex.Row * CellHeight, 0f);
            return startPosition + offset;
        }

        private MoveDirection GetNextMoveDirection()
        {
            var currentGridBounds = GetCurrentGridBounds();
            var canMoveRight = _currentGridIndex.Column + currentGridBounds.MaxColumn < GridColCount;
            var canMoveLeft = _currentGridIndex.Column + currentGridBounds.MinColumn > 0;
            var canMoveDown = _currentGridIndex.Row + currentGridBounds.MaxRow < GridRowCount;

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
            _ufoSpawnDuration = UFOConfig.SpawnDurationRange.GetRandomValue() / _currentSpeedMultiplier;
        }
    }
}