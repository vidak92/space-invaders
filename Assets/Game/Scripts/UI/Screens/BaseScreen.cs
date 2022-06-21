using SpaceInvaders.Common;
using SpaceInvaders.Common.Services;
using SpaceInvaders.Gameplay;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.UI.Screens
{
    public class BaseScreen : MonoBehaviour
    {
        // Event Handlers
        [SerializeField]
        protected GameObject _container;

        [Inject]
        protected UIController _uiController;
        [Inject]
        protected AppController _appController;
        [Inject]
        protected InputService _inputService;
        [Inject]
        protected GameStatsController _gameStatsController;
        [Inject]
        protected HighScoreService _highScoreService;

        [Inject]
        public virtual void Init()
        {
            gameObject.SetActive(true);
            Hide();
            OnInit();
            UpdateControlsForCurrentPlatform();
        }

        // Methods
        public void Show()
        {
            _container.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            _container.SetActive(false);
            OnHide();
        }

        protected virtual void OnInit() { }
        protected virtual void UpdateControlsForCurrentPlatform() { }
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}