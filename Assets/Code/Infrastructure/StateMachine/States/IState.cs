namespace Code.Infrastructure.StateMachine.States
{
    public interface IState
    {
        void Exit();
    }

    public interface IDefaultState : IState
    {
        void Enter();
    }

    public interface IPayloadState<TPayload> : IState
    {
        void Enter(TPayload payload);
    }
}