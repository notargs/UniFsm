using System;
using System.Collections.Generic;

namespace UniFsm
{
    public sealed class StateMachine<TState> : IDisposable
        where TState : Enum
    {
        private readonly Dictionary<TState, StateBehaviour<TState>> _stateDictionary = new Dictionary<TState, StateBehaviour<TState>>();
        
        private TState _currentState;
        private bool _isFirstFrame = true;

        public StateMachine(TState defaultState)
        {
            _currentState = defaultState;
        }

        public void RegisterState(TState stateType, StateBehaviour<TState> stateBehaviour)
        {
            _stateDictionary[stateType] = stateBehaviour;
        }
        
        public void Tick()
        {
            if (_isFirstFrame)
            {
                _stateDictionary[_currentState].OnEnabled();
                _isFirstFrame = false;
            }

            while (true)
            {
                var nextState = _stateDictionary[_currentState].Tick();
                if (!nextState.HasValue) break;
                
                _stateDictionary[_currentState].OnDisabled();
                _currentState = nextState.Value;
                _stateDictionary[_currentState].OnEnabled();
            }
        }

        public void Dispose()
        {
            _stateDictionary[_currentState].OnDisabled();
        }
    }
}