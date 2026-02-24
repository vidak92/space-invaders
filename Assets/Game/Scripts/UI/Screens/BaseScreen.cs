using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public class BaseScreen : MonoBehaviour
    {
        protected GameController GameController => ServiceLocator.Get<GameController>();
        protected UIController UIController => GameController.UIController;
        protected InputController InputController => GameController.InputController;
        protected HighScoreService HighScoreService => GameController.HighScoreService;
        protected GameConfig GameConfig => GameController.GameConfig;
        
        public virtual void Init()
        {
            gameObject.SetActive(true);
            Hide();
            OnInit();
            UpdateControlsForCurrentPlatform();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        protected virtual void OnInit() { }
        protected virtual void UpdateControlsForCurrentPlatform() { }
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}