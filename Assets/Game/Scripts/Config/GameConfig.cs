using System;
using System.Collections.Generic;
using SGSTools.Common;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class GameplayBounds
    {
        public float Left;
        public float Right;
        public float Bottom;
        public float Top;
    }

    [Serializable]
    public class ProjectileConfig
    {
        [Tooltip("In units/second.")]
        public float MoveSpeed;
        
        [Tooltip("Starting distance from the object that shoots.\n\nIn units.")]
        public float InitialVerticalOffset;

        public ProjectileDirection Direction;
        public LayerMask LayerMask;
    }

    [Serializable]
    public class PlayerConfig
    {
        [Tooltip("In units/second.")]
        public float MoveSpeed;

        [Tooltip("How long it takes for the player to reach full-speed.\n\nUse 0 for instant acceleration.\n\nIn seconds.")]
        public float AccelerationDuration;

        [Tooltip("Delay before the player can shoot again after firing.\n\nIn seconds.")]
        public float ShotCooldown;
        
        [Space]
        [Tooltip("Invincibility period after being shot.\n\nIn seconds.")]
        public float InvincibilityDuration;
        
        [Tooltip("Speed for a cosine blink animation.\n\nIn radians/second. ")]
        public float InvincibilityBlinkSpeed;
        
        [Tooltip("Color multiplier for the blink animation.\n\nUsed as min. value when interpolating.\n\nFull-white means no change.")]
        public Color InvincibilityMinColorTint;
        
        [Tooltip("Color multiplier for the blink animation.\n\nUsed as min. value when interpolating.\n\nFull-white means no change.")]
        public Color InvincibilityMaxColorTint;

        [Space]
        [Tooltip("Number of lives at the start of a game.\n\nEditing requires a game restart.")]
        public int StartLives;

        [Space]
        public ProjectileConfig ProjectileConfig;
    }

    [Serializable]
    public class EnemyScoreValue
    {
        public EnemyType Type;
        public int Points;
    }

    [Serializable]
    public class UFOConfig
    {
        public bool IsEnabled;

        [Tooltip("In units/second.")]
        public int MoveSpeed;
        
        [Tooltip("Multiplied by the wave's speed multiplier, to get a different rate of speedup than the rest of the enemies.")]
        public float SpeedMultiplier;

        [Space]
        public float BoundsLeft;
        public float BoundsRight;

        public FloatRange SpawnDurationRange;
    }

    [Serializable]
    public class EnemiesConfig
    {
        public int Enemy1ScoreValue;
        public int Enemy2ScoreValue;
        public int Enemy3ScoreValue;
        public int EnemyUFOScoreValue;
        
        [Space]
        public UFOConfig UFOConfig;

        [Space]
        public ProjectileConfig ProjectileConfig;
        
        public int GetScoreValueForEnemyType(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.Enemy1:
                    return Enemy1ScoreValue;
                case EnemyType.Enemy2:
                    return Enemy2ScoreValue;
                case EnemyType.Enemy3:
                    return Enemy3ScoreValue;
                case EnemyType.UFO:
                    return EnemyUFOScoreValue;
                default:
                    break;
            }
            // @TODO log error
            return 0;
        }
    }

    [Serializable]
    public class WaveConfig
    {
        public float NewWaveDelay;
        public FloatRange WaveSpeedMultiplierRange;
        
        [Space]
        public float StepDuration;
        public float IdleDuration;
        public FloatRange ShotDelayRange;

        [Space]
        public Vector2Int GridSize;
        public int EnemiesPerRow;
        public List<EnemyType> EnemyRows = new List<EnemyType>();
    }
    
    [Serializable]
    public class GameplayAssetsConfig
    {
        public Player Player;
        public Enemy Enemy1;
        public Enemy Enemy2;
        public Enemy Enemy3;
        public EnemyUFO UFO;
        public EnemyWave EnemyWave;
        public Projectile Projectile;

        public Enemy GetEnemyPrefab(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Enemy1:
                    return Enemy1;
                case EnemyType.Enemy2:
                    return Enemy2;
                case EnemyType.Enemy3:
                    return Enemy3;
                case EnemyType.UFO:
                    return UFO;
                default:
                    // @TODO log error
                    return null;
            }
        }
    }

    [CreateAssetMenu(menuName = "Config/Gameplay Config")]
    public class GameConfig : ScriptableObject
    {
        [Tooltip("Orthographic")]
        public float CameraSize;

        [Tooltip("Delay before 'game over' is shown after the player dies.\n\nIn seconds.")]
        public float ResultsScreenDelay;

        [Space]
        [Tooltip("Bounds within which Player and Enemy objects can move.\n\nIn units, relative to the camera.")]
        public GameplayBounds GameplayBounds;

        [Space]
        public PlayerConfig PlayerConfig;
        
        [Space]
        public EnemiesConfig EnemiesConfig;
        
        [Space]
        public WaveConfig WaveConfig;

        [Space]
        public GameplayAssetsConfig GameplayAssetsConfig;
    }
}