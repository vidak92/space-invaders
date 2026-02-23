using System;
using SGSTools.Extensions;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public enum ProjectileDirection
    {
        Up,
        Down
    }

    public class Projectile : MonoBehaviour
    {
        private GameConfig _gameConfig;

        private ProjectileConfig _projectileConfig;

        private Vector3 _direction;
        
        private GameController GameController => ServiceLocator.Get<GameController>(); // @TODO use callbacks instead
        
        public void Init(GameConfig gameConfig, ProjectileConfig projectileConfig, Vector3 sourcePosition)
        {
            _gameConfig = gameConfig;
            _projectileConfig = projectileConfig;
            
            var directionZ = _projectileConfig.Direction == ProjectileDirection.Up ? 1f : -1f;
            _direction = Vector3.up * directionZ;
            var angleZ = MathfExt.VectorXYToAngle(_direction) * Mathf.Rad2Deg - 90f;

            transform.position = sourcePosition + _direction * _projectileConfig.InitialVerticalOffset;
            transform.rotation = Quaternion.Euler(0f, 0f, angleZ);

            gameObject.layer = _projectileConfig.LayerMask.ToIndex();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage();
                GameController.DespawnProjectile(this);
            }
        }

        public void Update()
        {
            var dt = Time.deltaTime;
            transform.position += _projectileConfig.MoveSpeed * _direction * dt;

            var cameraSize = _gameConfig.CameraSize;
            var positionY = transform.position.y;
            if (positionY < -cameraSize || positionY > cameraSize)
            {
                GameController.DespawnProjectile(this);
            }
        }
    }
}