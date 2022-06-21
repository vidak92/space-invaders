using SpaceInvaders.Gameplay.Config;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Objects
{
    public class GameplayObject : MonoBehaviour
    {
        // Fields
        [Inject]
        protected GameplayConfig _gameplayConfig;

        // Properties
        public bool IsActive => gameObject.activeSelf;

        [Inject]
        protected virtual void Init()
        {
            ResetState();
        }

        // Methods
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void ResetState()
        {

        }
    }
}