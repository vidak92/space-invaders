namespace SpaceInvaders.Common.State
{
    public class LoadingState : BaseState
    {
        public LoadingState(AppController appController) : base(appController) { }

        // Methods
        public override void Enter()
        {
            Load();
        }

        private void Load()
        {
            _appController.SetState(GameState.MainMenu);
        }
    }
}