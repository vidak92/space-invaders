using MijanTools.Common;
using MijanTools.Components;
using SpaceInvaders.Common;
using SpaceInvaders.Util;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class Player : MonoBehaviour, IShootable
    {
        // Fields
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private GameplayConfig _gameplayConfig;
        private ObjectPool<Projectile> _projectilePool;
        private GameStatsController _gameStatsController;

        private float _speedX;
        private float _shotCooldownTimer;

        private bool _isInvincible;
        private float _invincibleTimer;

        // Properties
        public bool IsActive => gameObject.activeSelf;
        private PlayerConfig PlayerConfig => _gameplayConfig.PlayerConfig;
        private GameplayBounds GameplayBounds => _gameplayConfig.GameplayBounds;

        public Action OnPlayerDied { get; set; }
        
        public void Init(GameplayConfig gameplayConfig, ObjectPool<Projectile> projectilePool, GameStatsController gameStatsController)
        {
            _gameplayConfig = gameplayConfig;
            _projectilePool = projectilePool;
            _gameStatsController = gameStatsController;

            _speedX = 0f;
            _shotCooldownTimer = 0f;

            SetActive(false);
            ResetPosition();

            _gameStatsController.OnLifeLost += OnLifeLost;
        }

        // Unity Events
        private void OnTriggerEnter(Collider other)
        {
            var hittable = other.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.TakeHit();
                OnEnemyHit();
            }
        }

        // IShootable
        public void TakeDamage()
        {
            if (!_isInvincible)
            {
                _isInvincible = true;
                _invincibleTimer = 0f;
                _gameStatsController.OnPlayerShot();
            }
        }

        // Methods
        public void OnUpdate(PlayerInput playerInput)
        {
            var dt = Time.deltaTime;
            var targetSpeedX = playerInput.MoveDirectionY * PlayerConfig.MoveSpeed * dt;
            _speedX = targetSpeedX;
            transform.AddPositionX(_speedX);
            transform.ClampPositionX(GameplayBounds.Left, GameplayBounds.Right);

            if (_shotCooldownTimer > 0f)
            {
                _shotCooldownTimer -= dt;
            }

            if (playerInput.ShouldShoot && _shotCooldownTimer <= 0f)
            {
                _shotCooldownTimer = PlayerConfig.ShotCooldown;
                var projectile = _projectilePool.Get();
                projectile.Init(_gameplayConfig, _gameplayConfig.PlayerConfig.ProjectileConfig, transform.position);
            }

            if (_isInvincible)
            {
                var multiplier = Mathf.Cos(Time.time * PlayerConfig.InvincibilityBlinkSpeed);
                multiplier = Mathf.Abs(multiplier);
                var color = Color.Lerp(PlayerConfig.InvincibilityMinColorTint, PlayerConfig.InvincibilityMaxColorTint, multiplier);
                _meshRenderer.material.SetColor(Constants.MaterialColorPropertyName, color);

                _invincibleTimer += dt;
                if (_invincibleTimer > PlayerConfig.InvincibilityDuration)
                {
                    _isInvincible = false;
                    _invincibleTimer = 0f;
                    ResetColor();
                }
            }
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(0f, 0f, GameplayBounds.Bottom);
        }

        public void ResetColor()
        {
            _meshRenderer.material.SetColor(Constants.MaterialColorPropertyName, Color.white);
        }

        private void OnLifeLost(int remainingLives)
        {
            if (remainingLives == 0)
            {
                SetActive(false);
                OnPlayerDied?.Invoke();
            }
        }

        private void OnEnemyHit()
        {
            SetActive(false);
            OnPlayerDied?.Invoke();
        }
    }
}