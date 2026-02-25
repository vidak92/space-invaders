using SGSTools.Components;
using SGSTools.Util;
using UnityEngine;

namespace SpaceInvaders
{
    public class AppController : MonoBehaviour
    {
        public GameConfig GameConfig;
        
        [Space]
        public ObjectShaker CameraShaker;
        public Material VignetteMaterial; // @TODO move to GameConfig?
        
        [Space]
        public AudioController AudioController;
        public InputController InputController;
        public GameController GameController;
        public UIController UIController;

        [Space]
        public bool UseTouchControls;
        
        public HighScoreService HighScoreService { get; private set; } = new HighScoreService();
        
        private void Awake()
        {
            ServiceLocator.Add(this);
            ServiceLocator.Add(AudioController);
            ServiceLocator.Add(InputController);
            ServiceLocator.Add(GameController);
            ServiceLocator.Add(UIController);
            
            Application.targetFrameRate = 60;
            Camera.main.orthographicSize = GameConfig.CameraSize;
            
            DebugDraw.IsEnabled = false;
            DebugDraw.Settings.DefaultColor = Color.gray;
            
            InputController.Init();
            UIController.Init();
            ResetVignette();
            
            if (IsMobilePlatform())
            {
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.orientation = ScreenOrientation.LandscapeRight;
                Screen.fullScreen = true;
            }
        }
        
        private void OnDestroy()
        {
            ResetVignette();
        }
        
        public bool IsMobilePlatform()
        {
#if UNITY_EDITOR
            return UseTouchControls;
#endif
            return Application.isMobilePlatform;
        }
        
        public void UpdateVignette(float power, bool isAdditive)
        {
            VignetteMaterial.SetFloat("_VignettePower", power);
            VignetteMaterial.SetFloat("_IsAdditive", isAdditive ? 1f : 0f);
        }
        
        public void ResetVignette()
        {
            UpdateVignette(GameConfig.VignettePowerRange.Min, isAdditive: false);
        }
    }
}