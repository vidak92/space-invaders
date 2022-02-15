namespace SpaceInvaders.Common
{
    // Structs
    public enum GameState
    {
        Loading,
        MainMenu,
        Gameplay,
        Results,
        HighScores,
        Controls
    }

    public abstract class BaseState
    {
        // Fields
        protected AppController _appController;

        public BaseState(AppController appController)
        {
            _appController = appController;
        }

        // Methods
        public virtual void Enter() { }

        public virtual void OnUpdate() { }
        
        public virtual void Exit() { }
    }
}