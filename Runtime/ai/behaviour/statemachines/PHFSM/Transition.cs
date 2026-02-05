using System;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public class Transition
    {
        public PHS NextState { get; private set; }
        public void SetNextState(PHS nextState)
        {
            if (nextState == null) throw new ArgumentNullException(nameof(nextState));
            NextState = nextState;
        }

        public bool EnterCondition()
        {
            if (NextState == null) throw new ArgumentNullException(nameof(NextState));
            return NextState.EnterCondition;
        }
    }
}
