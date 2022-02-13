using SpaceInvaders.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SpaceInvaders.Gameplay
{
    public interface IValidatable
    {
        public void Validate();
    }

    [Serializable]
    public class GameplayBounds : IValidatable
    {
        public float Left;
        public float Right;
        public float Bottom;
        public float Top;

        public void Validate()
        {
            Assert.IsTrue(Right > Left, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(Right), nameof(Left)));
            Assert.IsTrue(Top > Bottom, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(Top), nameof(Bottom)));
        }
    }

    // Projectile
    [Serializable]
    public class ProjectileConfig : IValidatable
    {
        [Tooltip("In units/second.")]
        public float MoveSpeed;
        
        [Tooltip("Starting distance from the object that shoots.\n\nIn units.")]
        public float InitialVerticalOffset;

        public ProjectileDirection Direction;
        public LayerMask LayerMask;

        public void Validate()
        {
            Assert.IsTrue(MoveSpeed > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MoveSpeed), 0f));
        }
    }

    // Player
    [Serializable]
    public class PlayerConfig : IValidatable
    {
        [Tooltip("In units/second.")]
        public float MoveSpeed;

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
        public int StartLives;

        [Space]
        public ProjectileConfig ProjectileConfig;

        public void Validate()
        {
            Assert.IsTrue(StartLives > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(StartLives), 0f));

            Assert.IsTrue(MoveSpeed > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MoveSpeed), 0f));
            Assert.IsTrue(ShotCooldown > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(ShotCooldown), 0f));
            Assert.IsTrue(InvincibilityDuration > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(InvincibilityDuration), 0f));
            Assert.IsTrue(InvincibilityBlinkSpeed > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(InvincibilityBlinkSpeed), 0f));

            ProjectileConfig.Validate();
        }
    }

    // Enemies
    [Serializable]
    public class EnemyScoreValue : IValidatable
    {
        public EnemyType Type;
        public int Points;

        public void Validate()
        {
            Assert.IsTrue(Points > 0, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(Points), 0));
        }
    }

    [Serializable]
    public class UFOConfig : IValidatable
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

        public void Validate()
        {
            Assert.IsTrue(MoveSpeed > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MoveSpeed), 0f));
            Assert.IsTrue(SpeedMultiplier > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(SpeedMultiplier), 0f));

            Assert.IsTrue(BoundsRight > BoundsLeft, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(BoundsRight), nameof(BoundsLeft)));
            Assert.IsTrue(MaxSpawnDuration > MinSpawnDuration, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MaxSpawnDuration), nameof(MinSpawnDuration)));
        }
    }

    [Serializable]
    public class EnemiesConfig : IValidatable
    {
        public UFOConfig UFOConfig;

        [Space]
        public List<EnemyScoreValue> ScoreValues = new List<EnemyScoreValue>();

        [Space]
        public ProjectileConfig ProjectileConfig;

        public void Validate()
        {
            UFOConfig.Validate();

            var allEnemyTypesHaveScoreValues = 
                ContainsScoreValueForEnemyType(EnemyType.Enemy1) && 
                ContainsScoreValueForEnemyType(EnemyType.Enemy2) &&
                ContainsScoreValueForEnemyType(EnemyType.Enemy3) &&
                ContainsScoreValueForEnemyType(EnemyType.UFO);
            Assert.IsTrue(allEnemyTypesHaveScoreValues, "GameplayConfig: EnemiesConfig must have score values for all enemy types.");

            ProjectileConfig.Validate();
        }

        public int GetScoreValueForEnemyType(EnemyType enemyType)
        {
            foreach (var scoreValue in ScoreValues)
            {
                if (scoreValue.Type == enemyType)
                {
                    return scoreValue.Points;
                }
            }
            // Fallback, shouldn't happen.
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

    // Enemy Waves
    [Serializable]
    public class WaveConfig : IValidatable
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

        public void Validate()
        {
            Assert.IsTrue(StepDuration > 0, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(StepDuration), 0));
            Assert.IsTrue(IdleDuration > 0, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(IdleDuration), 0));

            Assert.IsTrue(MaxWaveSpeedMultiplier > MinWaveSpeedMultiplier, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MaxWaveSpeedMultiplier), nameof(MinWaveSpeedMultiplier)));
            
            Assert.IsTrue(MaxShotDelay > MinShotDelay, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MaxShotDelay), nameof(MinShotDelay)));

            Assert.IsTrue(MaxGridRows > EnemyRows.Count, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MaxGridRows), nameof(EnemyRows)));
            Assert.IsTrue(MaxGridColumns > EnemiesPerRow, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(MaxGridColumns), nameof(EnemiesPerRow)));
            Assert.IsTrue(EnemiesPerRow > 0, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(EnemiesPerRow), 0));
            Assert.IsTrue(EnemyRows.Count > 0, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(EnemyRows), 0));
        }
    }

    [CreateAssetMenu(menuName = "Config/Gameplay Config")]
    public class GameplayConfig : ScriptableObject, IValidatable
    {
        [Tooltip("Orthographic camera size.")]
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

        public void Validate()
        {
            Assert.IsTrue(CameraSize > 0f, Utils.GetAssertGreaterThanMessage(this.GetType(), nameof(CameraSize), 0f));
            GameplayBounds.Validate();
            PlayerConfig.Validate();
            EnemiesConfig.Validate();
            WaveConfig.Validate();
            Debug.Log($"{nameof(GameplayConfig)}: Validation successful.");
        }
    }
}