using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public class BaseScreen : MonoBehaviour
    {
        protected AppController AppController => ServiceLocator.Get<AppController>();
        protected GameController GameController => ServiceLocator.Get<GameController>();
        protected AudioController AudioController => ServiceLocator.Get<AudioController>();
        protected UIController UIController => ServiceLocator.Get<UIController>();
        protected InputController InputController => ServiceLocator.Get<InputController>();
        protected GameConfig GameConfig => AppController.GameConfig;
        
        public virtual void Init()
        {
            gameObject.SetActive(true);
            Hide();
            OnInit();
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
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}