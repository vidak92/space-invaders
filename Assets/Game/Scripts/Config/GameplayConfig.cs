using System;
using System.Collections.Generic;
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
        [Tooltip("Max horizontal offset allowed from the camera.\n\nIn units.")]
        public float BoundsLeft;

        [Tooltip("Max horizontal offset allowed from the camera.\n\nIn units.")]
        public float BoundsRight;

        [Space]
        [Tooltip("Lower bound for a random spawn duration.\n\nIn seconds.")]
        public float MinSpawnDuration;

        [Tooltip("Upper bound for a random spawn duration.\n\nIn seconds.")]
        public float MaxSpawnDuration;
    }

    [Serializable]
    public class EnemiesConfig
    {
        public UFOConfig UFOConfig;

        [Space]
        public List<EnemyScoreValue> ScoreValues = new List<EnemyScoreValue>();

        [Space]
        public ProjectileConfig ProjectileConfig;
        
        public int GetScoreValueForEnemyType(EnemyType enemyType)
        {
            foreach (var scoreValue in ScoreValues)
            {
                if (scoreValue.Type == enemyType)
                {
                    return scoreValue.Points;
                }
            }
            // @NOTE Fallback, shouldn't happen.
            return 0;
        }

        public bool ContainsScoreValueForEnemyType(EnemyType enemyType)
        {
            foreach (var scoreValue in ScoreValues)
            {
                if (scoreValue.Type == enemyType)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class WaveConfig
    {
        [Header("Timers & Speed")]
        [Tooltip("Delay before spawning a new wave after the current one is completed.\n\nIn seconds.")]
        public float NewWaveDelay;
        
        [Tooltip("How long it takes the wave formation to move one step.\n\nIn seconds.")]
        public float StepDuration;

        [Tooltip("How long the wave formation waits before starting a new step.\n\nIn seconds.")]
        public float IdleDuration;
        
        [Space]
        [Tooltip("Speed multiplier for when a wave formation is full.\n\nInterpolated inbetween full and empty.")]
        public float MinWaveSpeedMultiplier;

        [Tooltip("Speed multiplier for when a wave formation is empty.\n\nInterpolated inbetween full and empty.")]
        public float MaxWaveSpeedMultiplier;

        [Space]
        [Tooltip("In seconds.")]
        public float MinShotDelay;

        [Tooltip("In seconds.")]
        public float MaxShotDelay;

        [Header("Grid Config")]
        [Tooltip("Number of grid rows for the whole GamepayBounds area\n\nEditing requires a wave respawn or game restart.")]
        public int MaxGridRows;
        
        [Tooltip("Number of grid columns for the whole GamepayBounds area.\n\nEditing requires a wave respawn or game restart.")]
        public int MaxGridColumns;
        
        [Tooltip("Number of grid columns for the wave formation.\n\nEditing requires exiting and re-entering play mode.")]
        public int EnemiesPerRow;

        [Tooltip("Enemy types for rows int the wave formation.\n\nEditing requires exiting and re-entering play mode.")]
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
    public class GameplayConfig : ScriptableObject
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