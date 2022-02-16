using SpaceInvaders.Gameplay;

namespace SpaceInvaders.Common.State
{
    public class GameplayState : BaseState
    {
        // Fields
        private GameplayController _gameplayController;

        public GameplayState(AppController appController,
            GameplayController gameplayController)
            : base(appController)
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