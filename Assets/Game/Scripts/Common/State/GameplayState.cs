using SpaceInvaders.Gameplay;

namespace SpaceInvaders.Common
{
    public class GameplayState : BaseState
    {
        // Fields
        private GameplayController _gameplayController;

        public GameplayState(GameManager gameManager,
            GameplayController gameplayController)
            : base(gameManager)
        {
            _gameplayController = gameplayController;
        }


        // Overrides
        public override void Enter()
        {
            _gameplayController.StartGame();
        }

        public override void OnUpdate()
        {
            _gameplayController.OnUpdate();
        }

        public override void Exit()
        {
            _gameplayController.ExitGame();
        }
    }
}