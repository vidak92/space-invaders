using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public enum EnemyType
    {
        Enemy1,
        Enemy2,
        Enemy3,
        UFO
    }

    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyType _type; // @TODO make public property
        public Health Health;

        private GameController GameController => GameController.Instance;

        public void Init()
        {
            Health.OnDamageTaken += OnDamageTaken;
        }

        public void OnDamageTaken()
        {
            gameObject.SetActive(false);
            GameController.OnEnemyShot(_type);
        }
    }
}