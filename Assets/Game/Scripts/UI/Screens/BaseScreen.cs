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
        
        protected UIController _uiController;
        protected AppController _appController;
        protected InputService _inputService;
        protected GameStatsController _gameStatsController;
        protected HighScoreService _highScoreService;

        [Inject]
        public virtual void Init(UIController uiController, 
            AppController appController, 
            InputService inputService, 
            GameStatsController gameStatsController,
            HighScoreService highScoreService)
        {
            _uiController = uiController;
            _appController = appController;
            _inputService = inputService;
            _gameStatsController = gameStatsController;
            _highScoreService = highScoreService;

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