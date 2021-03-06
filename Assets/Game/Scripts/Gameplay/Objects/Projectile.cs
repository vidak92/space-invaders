using MijanTools.Common;
using MijanTools.Components;
using SpaceInvaders.Gameplay.Config;
using SpaceInvaders.Gameplay.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Objects
{
    // Structs
    [Serializable]
    public enum ProjectileDirection
    {
        Up,
        Down
    }

    public class Projectile : MonoBehaviour, MijanTools.Components.IPoolable<Projectile>
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Projectile> { }

        // Fields
        private GameplayConfig _gameplayConfig;

        private ProjectileConfig _projectileConfig;

        private Vector3 _direction;

        // Properties
        public ObjectPool<Projectile> Pool { get; set; }

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

        // Unity Events
        private void OnTriggerEnter(Collider other)
        {
            Pool.Return(this);

            var shootable = other.GetComponent<IShootable>();
            if (shootable != null)
            {
                shootable.TakeDamage();
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
                Pool.Return(this);
            }
        }
    }
}