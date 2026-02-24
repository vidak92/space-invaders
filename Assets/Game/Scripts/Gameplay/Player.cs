using System;
using System.Collections.Generic;
using DG.Tweening;
using SGSTools.Extensions;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public class Player : MonoBehaviour
    {
        public Collider2D[] Colliders;
        public Health Health;
        public SpriteRenderer SpriteRenderer;
        public ParticleSystem Particles;
        
        private float _moveSpeedX;
        private float _accelerationTimer;
        private readonly float _accelerationTimerEpsilon = 0.064f;
        
        private float _shotCooldownTimer;

        private bool _isInvincible;
        private float _invincibleTimer;

        private List<Color> _materialColors = new List<Color>();

        private Vector3 _startPosition = new Vector3(0f, -14f, 0f); // @TODO config

        public bool IsActive { get; private set; }

        private GameController GameController => ServiceLocator.Get<GameController>();
        private GameConfig GameConfig => GameController.GameConfig;
        private PlayerConfig PlayerConfig => GameConfig.PlayerConfig;
        private GameplayBounds GameplayBounds => GameConfig.GameplayBounds;

        public Action OnPlayerKilled { get; set; }
        
        public void Init()
        {
            ResetState();
            Health.OnDamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken()
        {
            if (!_isInvincible)
            {
                _isInvincible = true;
                _invincibleTimer = 0f;
                GameController.OnPlayerShot();
            }
        }
        
        public void SetActive(bool active)
        {
            IsActive = active;
            gameObject.SetActive(active);
        }

        public void ResetState()
        {
            transform.position = _startPosition;
            ResetColor();

            _moveSpeedX = 0f;
            _accelerationTimer = 0f;
            _shotCooldownTimer = 0f;

            _isInvincible = false;
            _invincibleTimer = 0f;

            SetActive(false);
            SetCollidersEnabled(true);
        }

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

            var positionX = Mathf.Clamp(transform.position.x + _moveSpeedX, GameplayBounds.Left, GameplayBounds.Right);
            transform.SetPositionX(positionX);

            // Shot Update
            if (_shotCooldownTimer > 0f)
            {
                _shotCooldownTimer -= dt;
            }

            if (playerInput.ShouldShoot && _shotCooldownTimer <= 0f)
            {
                _shotCooldownTimer = PlayerConfig.ShotCooldown;
                GameController.SpawnProjectile(PlayerConfig.ProjectileConfig, transform.position);
            }

            // Invincibility Update
            if (_isInvincible)
            {
                var multiplier = Mathf.Cos(Time.time * PlayerConfig.InvincibilityBlinkSpeed);
                multiplier = Mathf.Abs(multiplier);
                var color = Color.Lerp(PlayerConfig.InvincibilityColorTint, Color.white, multiplier);
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
            SetColorMultiplier(Color.white);
        }

        private void SetColorMultiplier(Color color)
        {
            SpriteRenderer.color = color;
        }
        
        public void OnLifeLost(int remainingLives)
        {
            if (remainingLives == 0)
            {
                SetCollidersEnabled(false);
                SpriteRenderer.enabled = false;
                Particles.Play();

                var particleLifetime = Particles.main.startLifetime.constantMax;
                DOVirtual.DelayedCall(particleLifetime, () =>
                {
                    SetCollidersEnabled(true);
                    SpriteRenderer.enabled = true;
                    SetActive(false);
                    OnPlayerKilled?.Invoke();
                });
            }
        }

        private void SetCollidersEnabled(bool enabled)
        {
            foreach (var collider in Colliders)
            {
                collider.enabled = enabled;
            }
        }
    }
}