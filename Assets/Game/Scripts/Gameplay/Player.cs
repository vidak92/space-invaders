using System;
using System.Collections.Generic;
using SGSTools.Extensions;
using UnityEngine;

namespace SpaceInvaders
{
    public class Player : MonoBehaviour
    {
        public Health Health;
        
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private float _moveSpeedX;
        private float _accelerationTimer;
        private readonly float _accelerationTimerEpsilon = 0.064f;
        
        private float _shotCooldownTimer;

        private bool _isInvincible;
        private float _invincibleTimer;

        private List<Color> _materialColors = new List<Color>();

        private GameController GameController => GameController.Instance;
        private GameplayConfig GameplayConfig => GameController.GameplayConfig;
        private PlayerConfig PlayerConfig => GameplayConfig.PlayerConfig;
        private GameplayBounds GameplayBounds => GameplayConfig.GameplayBounds;

        public Action OnPlayerDied { get; set; }
        
        public void Init()
        {
            ResetState();
            InitMaterialColors();

            Health.OnDamageTaken += OnDamageTaken;
        }

        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage();
                OnEnemyHit();
            }
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

        public void ResetState()
        {
            transform.position = new Vector3(0f, 0f, GameplayBounds.Bottom);
            ResetColor();

            _moveSpeedX = 0f;
            _accelerationTimer = 0f;
            _shotCooldownTimer = 0f;

            _isInvincible = false;
            _invincibleTimer = 0f;
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

        public void OnLifeLost(int remainingLives)
        {
            if (remainingLives == 0)
            {
                gameObject.SetActive(false);
                OnPlayerDied?.Invoke();
            }
        }

        private void OnEnemyHit()
        {
            gameObject.SetActive(false);
            OnPlayerDied?.Invoke();
        }
    }
}