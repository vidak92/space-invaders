using MijanTools.Common;
using MijanTools.Components;
using SpaceInvaders.Common;
using SpaceInvaders.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay
{
    public class Player : GameplayObject, IShootable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Player> { }

        // Fields
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private ObjectPool<Projectile> _projectilePool;
        private GameStatsController _gameStatsController;

        private float _speedX;
        private float _shotCooldownTimer;

        private bool _isInvincible;
        private float _invincibleTimer;

        private List<Color> _materialColors = new List<Color>();

        // Properties
        private PlayerConfig PlayerConfig => _gameplayConfig.PlayerConfig;
        private GameplayBounds GameplayBounds => _gameplayConfig.GameplayBounds;

        public Action OnPlayerDied { get; set; }
        
        [Inject]
        private void Init(GameplayConfig gameplayConfig, ObjectPool<Projectile> projectilePool, GameStatsController gameStatsController)
        {
            base.Init(gameplayConfig);

            _projectilePool = projectilePool;
            _gameStatsController = gameStatsController;

            ResetState();
            InitMaterialColors();
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

        // Overrides
        public override void ResetState()
        {
            ResetPosition();
            ResetColor();

            _speedX = 0f;
            _shotCooldownTimer = 0f;

            _isInvincible = false;
            _invincibleTimer = 0f;
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
                SetColorMultiplier(color);

                _invincibleTimer += dt;
                if (_invincibleTimer > PlayerConfig.InvincibilityDuration)
                {
                    _isInvincible = false;
                    _invincibleTimer = 0f;
                    ResetColor();
                }
            }
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(0f, 0f, GameplayBounds.Bottom);
        }

        private void ResetColor()
        {
            SetColorMultiplier(PlayerConfig.InvincibilityMaxColorTint);
        }

        private void SetColorMultiplier(Color color)
        {
            for (int i = 0; i < _materialColors.Count; i++)
            {
                if (_meshRenderer.materials.ContainsIndex(i))
                {
                    _meshRenderer.materials[i].color = _materialColors[i] * color;
                }
            }
        }

        private void InitMaterialColors()
        {
            foreach (var material in _meshRenderer.materials)
            {
                _materialColors.Add(material.color);
            }
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