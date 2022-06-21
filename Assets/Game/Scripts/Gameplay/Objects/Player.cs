using MijanTools.Common;
using MijanTools.Components;
using SpaceInvaders.Common.Services;
using SpaceInvaders.Gameplay.Config;
using SpaceInvaders.Gameplay.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Objects
{
    public class Player : GameplayObject, IShootable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Player> { }

        // Fields
        [SerializeField]
        private MeshRenderer _meshRenderer;

        [Inject]
        private ObjectPool<Projectile> _projectilePool;
        [Inject]
        private GameStatsController _gameStatsController;

        private float _moveSpeedX;
        private float _accelerationTimer;
        private readonly float _accelerationTimerEpsilon = 0.064f;
        
        private float _shotCooldownTimer;

        private bool _isInvincible;
        private float _invincibleTimer;

        private List<Color> _materialColors = new List<Color>();

        // Properties
        private PlayerConfig PlayerConfig => _gameplayConfig.PlayerConfig;
        private GameplayBounds GameplayBounds => _gameplayConfig.GameplayBounds;

        public Action OnPlayerDied { get; set; }
        
        [Inject]
        protected override void Init()
        {
            base.Init();

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
            transform.position = new Vector3(0f, 0f, GameplayBounds.Bottom);
            ResetColor();

            _moveSpeedX = 0f;
            _accelerationTimer = 0f;
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

            // Move Update
            if (PlayerConfig.AccelerationDuration > 0f)
            {
                // Accelerate/decelrate.
                if (playerInput.MoveDirectionX > 0f)
                {
                    // Moving right.
                    if (_accelerationTimer < PlayerConfig.AccelerationDuration)
                    {
                        _accelerationTimer += dt;
                    }
                }
                else if (playerInput.MoveDirectionX < 0f)
                {
                    // Moving left.
                    if (_accelerationTimer > -PlayerConfig.AccelerationDuration)
                    {
                        _accelerationTimer -= dt;
                    }
                }
                else
                {
                    // Not moving.
                    if (_accelerationTimer < -_accelerationTimerEpsilon)
                    {
                        _accelerationTimer += dt;
                    }
                    else if (_accelerationTimer > _accelerationTimerEpsilon)
                    {
                        _accelerationTimer -= dt;
                    }

                    // Clamp velocity if it's close to 0 and there's no input.
                    if (Mathf.Abs(_accelerationTimer) < _accelerationTimerEpsilon) { _accelerationTimer = 0f; }
                }

                // Update movment speed. Values for t mean:
                // -1: Full-speed left.
                // 1: Full-speed right.
                // 0: No movement.
                var t = Mathf.Clamp(_accelerationTimer / PlayerConfig.AccelerationDuration, -1f, 1f);

                // Convert t to [0, 1] range.
                t = (t + 1f) / 2f;

                // Lerp speed based on t.
                var maxSpeedX = PlayerConfig.MoveSpeed * dt;
                _moveSpeedX = Mathf.Lerp(-maxSpeedX, maxSpeedX, t);
            }
            else
            {
                // Change speed instantly.
                var targetSpeedX = playerInput.MoveDirectionX * PlayerConfig.MoveSpeed * dt;
                _moveSpeedX = targetSpeedX;
            }
            
            transform.AddPositionX(_moveSpeedX);
            transform.ClampPositionX(GameplayBounds.Left, GameplayBounds.Right);

            // Shot Update
            if (_shotCooldownTimer > 0f)
            {
                _shotCooldownTimer -= dt;
            }

            if (playerInput.ShouldShoot && _shotCooldownTimer <= 0f)
            {
                _shotCooldownTimer = PlayerConfig.ShotCooldown;
                var projectile = _projectilePool.Get();
                projectile.Init(_gameplayConfig, PlayerConfig.ProjectileConfig, transform.position);
            }

            // Invincibility Update
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