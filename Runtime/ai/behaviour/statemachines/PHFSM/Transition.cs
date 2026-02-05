using System;
using System.Collections;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public class Transition
    {
        public PHS NextState { get; }

        public Transition(PHS nextState)
        {
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
        }

        public bool EnterCondition() => NextState.EnterCondition;
    }
}
