using SGSTools.Common;
using SGSTools.Extensions;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public class EnemyUFO : MonoBehaviour
    {
        public Enemy Enemy;
        
        private MoveDirection _moveDirection;
        private Timer _spawnTimer;
        private FloatRange _spawnDurationRange;

        private GameController GameController => ServiceLocator.Get<GameController>();
        private GameConfig GameConfig => GameController.GameConfig;
        private UFOConfig UFOConfig => GameConfig.EnemiesConfig.UFOConfig;
        private GameplayBounds GameplayBounds => GameConfig.GameplayBounds;

        public void Init()
        {
            _spawnDurationRange = new FloatRange(UFOConfig.SpawnDurationRangeMin.Min, UFOConfig.SpawnDurationRangeMin.Max);
            var spawnDuration = _spawnDurationRange.GetRandomValue();
            _spawnTimer.Init(spawnDuration);
            _spawnTimer.Reset();

            Enemy.Init($"{EnemyType.UFO}");
        }

        public void OnWaveStart()
        {
            _spawnTimer.Reset();
        }

        public void OnUpdate(float formationSpeedT)
        {
            var dt = Time.deltaTime;
            
            if (Enemy.IsActive)
            {
                // @TODO let physics engine handle movement
                var directionX = _moveDirection == MoveDirection.Right ? 1f : -1f;
                var moveSpeed = UFOConfig.MoveSpeedRange.GetValueAt(formationSpeedT);
                transform.AddPositionX(directionX * moveSpeed * dt);

                if (transform.position.x < UFOConfig.BoundsLeft || transform.position.x > UFOConfig.BoundsRight)
                {
                    // despawn
                    Enemy.SetActive(false);
                    _spawnDurationRange.Min = UFOConfig.SpawnDurationRangeMin.GetValueAt(formationSpeedT);
                    _spawnDurationRange.Max = UFOConfig.SpawnDurationRangeMax.GetValueAt(formationSpeedT);
                    var spawnDuration = _spawnDurationRange.GetRandomValue();
                    _spawnTimer.Init(spawnDuration);
                    _spawnTimer.Reset();
                }
            }
            else
            {
                if (_spawnTimer.Update(dt))
                {
                    // spawn
                    Enemy.SetActive(true);
                    float positionX;
                    if (Random.Range(-1f, 1f) > 0f)
                    {
                        _moveDirection = MoveDirection.Right;
                        positionX = UFOConfig.BoundsLeft;
                    }
                    else
                    {
                        _moveDirection = MoveDirection.Left;
                        positionX = UFOConfig.BoundsRight;
                    }
                    
                    var offsetY = 2.2f; // @TODO config
                    transform.position = new Vector3(positionX, GameplayBounds.Top + offsetY, 0f);
                }
            }
        }
    }
}