using MijanTools.Common;
using SpaceInvaders.Common.State;
using SpaceInvaders.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Common
{
    public class AppController : MonoBehaviour
    {
        // Fields
        [Inject]
        private UIController _uiController;

        [Inject]
        private LoadingState _loadingState;
        [Inject]
        private MainMenuState _mainMenuState;
        [Inject]
        private GameplayState _gameplayState;
        [Inject]
        private ResultsState _resultsState;
        [Inject]
        private HighScoresState _highScoresState;
        [Inject]
        private ControlsState _controlsState;

        private readonly int _targetFrameRate = 60;

        private GameState _currentState;
        private Dictionary<GameState, BaseState> _allStates;

        // Properties
        private BaseState CurrentState => _allStates.HasKey(_currentState) ? _allStates[_currentState] : null;

        [Inject]
        private void Init()
        {
            _allStates = new Dictionary<GameState, BaseState>
            {
                [GameState.Loading] = _loadingState,
                [GameState.MainMenu] = _mainMenuState,
                [GameState.Gameplay] = _gameplayState,
                [GameState.Results] = _resultsState,
                [GameState.HighScores] = _highScoresState,
                [GameState.Controls] = _controlsState
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