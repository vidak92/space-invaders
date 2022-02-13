using MijanTools.Common;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class EnemyUFO : Enemy
    {
        // Fields
        private MoveDirection _moveDirection;

        // Properties
        private UFOConfig UFOConfig => _gameplayConfig.EnemiesConfig.UFOConfig;
        private GameplayBounds GameplayBounds => _gameplayConfig.GameplayBounds;

        // Methods
        public void OnUpdate(float waveSpeedMultiplier)
        {
            var directionX = _moveDirection == MoveDirection.Right ? 1f : -1f;
            var speedMultiplier = UFOConfig.SpeedMultiplier * waveSpeedMultiplier;
            transform.AddPositionX(directionX * UFOConfig.MoveSpeed * speedMultiplier * Time.deltaTime);

            if (transform.position.x < UFOConfig.BoundsLeft || transform.position.x > UFOConfig.BoundsRight)
            {
                SetAcitve(false);
            }
        }

        public void Spawn()
        {
            SetAcitve(true);
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
            transform.position = new Vector3(positionX, 0f, GameplayBounds.Top);
        }
    }
}