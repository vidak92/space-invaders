using System.Collections;
using UnityEngine;

namespace SpaceInvaders.Common
{
    public class LoadingState : BaseState
    {
        public LoadingState(GameManager gameManager) : base(gameManager) { }

        // Methods
        public override void Enter()
        {
            Load();
        }

        private void Load()
        {
            _gameManager.SetState(GameState.MainMenu);
        }

        //private IEnumerator LoadingCoroutine()
        //{
        //    yield return new WaitForSeconds(1f);
        //}
    }
}