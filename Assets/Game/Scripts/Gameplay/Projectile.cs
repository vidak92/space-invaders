using System;
using SGSTools.Extensions;
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
        private GameplayConfig _gameplayConfig;

        private ProjectileConfig _projectileConfig;

        private Vector3 _direction;
        
        private GameController GameController => GameController.Instance;
        
        public void Init(GameplayConfig gameplayConfig, ProjectileConfig projectileConfig, Vector3 sourcePosition)
        {
            _gameplayConfig = gameplayConfig;
            _projectileConfig = projectileConfig;
            
            var directionZ = _projectileConfig.Direction == ProjectileDirection.Up ? 1f : -1f;
            _direction = Vector3.forward * directionZ;

            transform.position = sourcePosition + _direction * _projectileConfig.InitialVerticalOffset;
            transform.LookAt(transform.position + _direction);

            gameObject.layer = _projectileConfig.LayerMask.ToIndex();
        }

        private void OnTriggerEnter(Collider other)
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

            var cameraSize = _gameplayConfig.CameraSize;
            var positionZ = transform.position.z;
            if (positionZ < -cameraSize || positionZ > cameraSize)
            {
                GameController.DespawnProjectile(this);
            }
        }
    }
}