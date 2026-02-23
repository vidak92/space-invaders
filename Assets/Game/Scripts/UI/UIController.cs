using UnityEngine;

namespace SpaceInvaders
{
    public class UIController : MonoBehaviour
    {
        public Canvas Canvas;
        
        public LoadingScreen LoadingScreen;
        public MainMenuScreen MainMenuScreen;
        public GameplayScreen GameplayScreen;
        public ResultsScreen ResultsScreen;
        public HighScoresScreen HighScoresScreen;
        public ControlsScreen ControlsScreen;

        public BaseScreen ActiveScreen { get; private set; }

        public void Init()
        {
            Canvas.worldCamera = Camera.main;
            Canvas.sortingLayerName = SortingLayers.Overlay;

            LoadingScreen.Init();
            MainMenuScreen.Init();
            GameplayScreen.Init();
            ResultsScreen.Init();
            HighScoresScreen.Init();
            ControlsScreen.Hide();
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
    }
}