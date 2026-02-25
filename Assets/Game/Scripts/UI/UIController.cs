using System;
using DG.Tweening;
using SGSTools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class UIController : MonoBehaviour
    {
        public Canvas Canvas;
        
        public MainMenuScreen MainMenuScreen;
        public GameplayScreen GameplayScreen;
        public GameOverScreen GameOverScreen;
        public HighScoresScreen HighScoresScreen;
        public ControlsScreen ControlsScreen;

        [Space]
        public Image TransitionImage;

        public BaseScreen ActiveScreen { get; private set; }
        public bool IsTransitioning { get; private set; }

        public void Init()
        {
            Canvas.worldCamera = Camera.main;
            Canvas.sortingLayerName = SortingLayers.Overlay;

            MainMenuScreen.Init();
            GameplayScreen.Init();
            GameOverScreen.Init();
            HighScoresScreen.Init();
            ControlsScreen.Init();

            TransitionImage.SetAlpha(1f);
            TransitionImage.gameObject.SetActive(true);
        }

        public void SetActiveScreen(BaseScreen screen)
        {
            if (ActiveScreen != null)
            {
                ActiveScreen.Hide();
            }
            ActiveScreen = screen;
            if (ActiveScreen != null)
            {
                ActiveScreen.Show();
            }
        }

        public void StartScreenTransition(BaseScreen toScreen, Action onComplete = null)
        {
            IsTransitioning = true;
            // TransitionImage.SetAlpha(0f);
            TransitionImage.gameObject.SetActive(true);
            
            var seq = DOTween.Sequence();
            seq.Append(TransitionImage.DOFade(1f, 0.25f));
            seq.AppendCallback(() =>
            {
                SetActiveScreen(toScreen);
                onComplete?.Invoke();
            });
            seq.Append(TransitionImage.DOFade(0f, 0.25f));
            seq.AppendCallback(() =>
            {
                IsTransitioning = false;
                TransitionImage.gameObject.SetActive(false);
            });
        }
    }
}