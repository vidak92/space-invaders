using SGSTools.Extensions;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public class EnemyUFO : Enemy
    {
        private MoveDirection _moveDirection;

        private GameController GameController => ServiceLocator.Get<GameController>();
        private GameConfig GameConfig => GameController.gameConfig;
        private UFOConfig UFOConfig => GameConfig.EnemiesConfig.UFOConfig;
        private GameplayBounds GameplayBounds => GameConfig.GameplayBounds;

        public void OnUpdate(float waveSpeedMultiplier)
        {
            var directionX = _moveDirection == MoveDirection.Right ? 1f : -1f;
            var speedMultiplier = UFOConfig.SpeedMultiplier * waveSpeedMultiplier;
            transform.AddPositionX(directionX * UFOConfig.MoveSpeed * speedMultiplier * Time.deltaTime);

            if (transform.position.x < UFOConfig.BoundsLeft || transform.position.x > UFOConfig.BoundsRight)
            {
                gameObject.SetActive(false);
            }
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
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
            transform.position = new Vector3(positionX, GameplayBounds.Top, 0f);
        }
    }
}