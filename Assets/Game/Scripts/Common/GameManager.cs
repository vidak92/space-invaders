using SpaceInvaders.UI;
using SpaceInvaders.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Common
{
    public class GameManager : MonoBehaviour
    {
        // Fields
        private UIController _uiController;

        private LoadingState _loadingState;
        private MainMenuState _mainMenuState;
        private GameplayState _gameplayState;
        private ResultsState _resultsState;
        private HighScoresState _highScoresState;

        private readonly int _targetFrameRate = 60;

        private GameState _currentState;
        private Dictionary<GameState, BaseState> _allStates;

        // Properties
        private BaseState CurrentState => _allStates.HasKey(_currentState) ? _allStates[_currentState] : null;

        [Inject]
        private void Init(UIController uiController,
            LoadingState loadingState,
            MainMenuState mainMenuState,
            GameplayState gameplayState,
            ResultsState resultsState,
            HighScoresState highScoresState)
        {
            _uiController = uiController;

            _loadingState = loadingState;
            _mainMenuState = mainMenuState;
            _gameplayState = gameplayState;
            _resultsState = resultsState;
            _highScoresState = highScoresState;

            _allStates = new Dictionary<GameState, BaseState>
            {
                [GameState.Loading] = _loadingState,
                [GameState.MainMenu] = _mainMenuState,
                [GameState.Gameplay] = _gameplayState,
                [GameState.Results] = _resultsState,
                [GameState.HighScores] = _highScoresState
            };

            Application.targetFrameRate = _targetFrameRate;
            SetState(GameState.Loading);
        }

        // Unity Events
        private void Update()
        {
            CurrentState?.OnUpdate();
        }

        public void SetState(GameState gameState)
        {
            CurrentState?.Exit();

            _currentState = gameState;
            _uiController.SetState(_currentState);
            
            CurrentState?.Enter();
        }
    }
}