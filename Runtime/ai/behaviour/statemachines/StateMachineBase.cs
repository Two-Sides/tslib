using System;
using System.Collections.Generic;

namespace TSLib.AI.Behaviour.StateMachines
{
    public abstract class StateMachineBase : IStateMachine
    {
        public bool Started { get; protected set; }
        public bool Running { get; protected set; }
        public IEqualityComparer<State> StateComparer { get; protected set; }

        public abstract void Execute(float deltaTime);

        public abstract void TransitionTo(State newState, bool doEnter = true, bool doExit = true, bool allowSameState = false);

        public virtual void RevertToPrevious(State previousState, bool doEnter = false,
            bool doExit = true, bool allowSameState = false)
        {
            TransitionTo(previousState, doEnter, doExit, allowSameState);
        }

        public virtual bool IsSameState(State s1, State s2) => StateComparer.Equals(s1, s2);

        public abstract void Start(bool doEnter = true);

        public virtual void Run()
        {
            if (!Started)
                throw new InvalidOperationException(
                    $"(not started) Before running the State Machine must be started.");
            Running = true;
        }

        public virtual void Stop()
        {
            Started = false;
            Running = false;
        }

        public virtual void Pause() => Running = false;
        public virtual void Resume() => Running = true;
    }
}
