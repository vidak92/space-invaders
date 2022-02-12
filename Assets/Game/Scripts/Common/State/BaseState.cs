namespace SpaceInvaders.Common
{
    // Structs
    public enum GameState
    {
        Loading,
        MainMenu,
        Gameplay,
        Results,
        HighScores
    }

    public abstract class BaseState
    {
        // Fields
        protected GameManager _gameManager;

        public BaseState(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        // Methods
        public virtual void Enter() { }

        public virtual void OnUpdate() { }
        
        public virtual void Exit() { }
    }
}