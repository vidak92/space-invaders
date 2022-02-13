using System;
using UnityEngine;
using Zenject;

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

    public class Enemy : GameplayObject, IShootable, IHittable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Enemy> { }

        // Fields
        [SerializeField]
        private EnemyType _type;

        protected GameStatsController _gameStatsController;

        [Inject]
        public void Init(GameplayConfig gameplayConfig, GameStatsController gameStatsController)
        {
            base.Init(gameplayConfig);

            _gameStatsController = gameStatsController;
        }

        // IShootable
        public void TakeDamage()
        {
            SetActive(false);
            _gameStatsController.OnEnemyShot(_type);
        }

        // IHittable
        public void TakeHit()
        {
            SetActive(false);
            _gameStatsController.OnEnemyShot(_type);
        }
    }
}