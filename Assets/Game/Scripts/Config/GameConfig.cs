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
        public float MoveSpeed;
        public float InitialVerticalOffset;
        public ProjectileDirection Direction;
        public LayerMask LayerMask;
    }

    [Serializable]
    public class PlayerConfig
    {
        public float MoveSpeed;
        public float AccelerationDuration;
        public float ShotCooldown;
        
        [Space]
        public float InvincibilityDuration;
        public float InvincibilityBlinkSpeed;
        public Color InvincibilityColorTint;

        [Space]
        [Range(1, 5)]
        public int StartLives;

        [Space]
        public ProjectileConfig ProjectileConfig;
    }

    [Serializable]
    public class UFOConfig
    {
        public bool IsEnabled; // @TODO is this necessary? maybe for debugging
        
        [Space]
        public FloatRange MoveSpeedRange;
        
        [Space]
        public float BoundsLeft;
        public float BoundsRight;

        [Space]
        public FloatRange SpawnDurationRangeMin;
        public FloatRange SpawnDurationRangeMax;
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
        public float SpawnDuration;
        public float FormationSpeedTPower;
        
        [Space]
        public FloatRange StepDurationRange;
        public FloatRange IdleDurationRange;
        
        [Space]
        public FloatRange ShotDelayRangeMin;
        public FloatRange ShotDelayRangeMax;

        [Space]
        public Vector2Int GridSize;
        public int EnemiesPerRow;
        public List<EnemyType> EnemyRows = new List<EnemyType>();
    }

    [CreateAssetMenu(menuName = "Config/Gameplay Config")]
    public class GameConfig : ScriptableObject
    {
        public float CameraSize;
        public float ResultsScreenDelay;
        
        [Header("Prefabs")]
        public Player PlayerPrefab;
        public Enemy Enemy1Prefab;
        public Enemy Enemy2Prefab;
        public Enemy Enemy3Prefab;
        public EnemyUFO UFOPrefab;
        public Projectile ProjectilePrefab;
        
        [Space]
        public GameplayBounds GameplayBounds;

        [Space]
        public PlayerConfig PlayerConfig;
        
        [Space]
        public EnemiesConfig EnemiesConfig;
        
        [Space]
        public WaveConfig WaveConfig;
        
        public Enemy GetEnemyPrefab(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Enemy1:
                    return Enemy1Prefab;
                case EnemyType.Enemy2:
                    return Enemy2Prefab;
                case EnemyType.Enemy3:
                    return Enemy3Prefab;
                default:
                    // @TODO log error
                    return null;
            }
        }
    }
}