using SpaceInvaders.Gameplay.Config;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Objects
{
    public class GameplayObject : MonoBehaviour
    {
        // Fields
        protected GameplayConfig _gameplayConfig;

        // Properties
        public bool IsActive => gameObject.activeSelf;

        protected void Init(GameplayConfig gameplayConfig)
        {
            _gameplayConfig = gameplayConfig;

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