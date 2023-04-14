using System;
using System.Collections.Generic;
using Code.Infrastructure.StateMachine.States;

namespace Code.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;

        private IState _activeState;

        public GameStateMachine(BootstrapState bootstrapState, LoadSceneState loadSceneState,
            GameLoopState gameLoopState, ExitState exitState)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = bootstrapState,
                [typeof(LoadSceneState)] = loadSceneState,
                [typeof(GameLoopState)] = gameLoopState,
                [typeof(ExitState)] = exitState,
            };

            bootstrapState.InitStateMachine(this);
            loadSceneState.InitStateMachine(this);
            gameLoopState.InitStateMachine(this);
        }

        public void Enter<TState>() where TState : class, IDefaultState
        {
            IDefaultState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}