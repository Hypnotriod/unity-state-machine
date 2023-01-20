using System;

namespace Assets.Scripts.StateMachine
{
    public interface IStateHandler
    {
        bool IsCompleted { get; }

        bool IsAborted { get; }

        bool IsStarted { get; }

        bool IsSuspended { get; }

        void RegisterAbortHandler(ActionHandler actionHandler, Action action);
    }
}
