using System;
using System.Collections.Generic;
using TSLib.AI.Behaviour.StateMachines.HFSM;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public abstract class PriorityHierarchicalState : HierarchicalState
    {
        public abstract int Priority { get; }
        public abstract bool IsInterruptible { get; }

        public List<Transition> Transitions { get; } = new();

        public sealed override void Execute(IStateMachine stateMachine, float deltaTime)
        {
            // State logic.
            Update(deltaTime);

            // Checking transitions.

            if (!ExitCondition() && !IsInterruptible) return;

            for (int i = 0; i < Transitions.Count; i++)
            {
                var transition = Transitions[i];

                if (!transition.ConditionMet()) continue;

                stateMachine.TransitionTo(transition.NextState);

                break;
            }
        }

        protected virtual void Update(float deltaTime) { }
        public abstract bool EnterCondition();
        public abstract bool ExitCondition();

        public void Add(Transition transition)
        {
            if (transition == null) return;
            if (transition.NextState == null) return;

            Transitions.Add(transition);
        }

        public void AddTransitions(ICollection<Transition> transitions, IComparer<Transition> comparer = null)
        {
            if (transitions == null) return;
            if (transitions.Count <= 0) return;

            Transitions.AddRange(transitions);

            if (comparer == null) return;
            SortTransitions(comparer);
        }

        public void SortTransitions(IComparer<Transition> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            Transitions.Sort(comparer);
        }
    }
}
