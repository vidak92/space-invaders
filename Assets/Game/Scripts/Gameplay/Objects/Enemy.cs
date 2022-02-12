using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    // Structs
    [Serializable]
    public enum EnemyType
    {
        Enemy1,
        Enemy2,
        Enemy3,
        UFO
    }

    public class Enemy : MonoBehaviour, IShootable, IHittable
    {
        // Fields
        [SerializeField]
        private EnemyType _type;

        protected GameplayConfig _gameplayConfig;
        protected GameStatsController _gameStatsController;

        // Properties
        public bool IsActive => gameObject.activeSelf;

        public void Init(GameplayConfig gameplayConfig, GameStatsController gameStatsController)
        {
            _gameplayConfig = gameplayConfig;
            _gameStatsController = gameStatsController;

            SetAcitve(false);
        }

        // Methods
        public void SetAcitve(bool active)
        {
            gameObject.SetActive(active);
        }

        // IShootable
        public void TakeDamage()
        {
            SetAcitve(false);
            _gameStatsController.OnEnemyShot(_type);
        }

        // IHittable
        public void TakeHit()
        {
            SetAcitve(false);
            _gameStatsController.OnEnemyShot(_type);
        }
    }
}