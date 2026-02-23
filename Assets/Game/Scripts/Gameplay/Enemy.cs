using System;
using SGSTools.Util;
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

        private GameController GameController => ServiceLocator.Get<GameController>();

        public void Init(string name)
        {
            gameObject.name = name;
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
            
            Health.OnDamageTaken += OnDamageTaken;
        }

        public void OnDamageTaken()
        {
            gameObject.SetActive(false);
            GameController.OnEnemyShot(_type);
        }
    }
}