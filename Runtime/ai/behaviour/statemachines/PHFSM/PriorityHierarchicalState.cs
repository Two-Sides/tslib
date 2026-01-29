using System;
using System.Collections.Generic;
using TSLib.AI.Behaviour.StateMachines.HFSM;
using TSLib.Utility.Patterns.EventChannels.Primitive;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public abstract class PriorityHierarchicalState : HierarchicalState
    {
        protected bool enterCondition = false;
        protected bool exitCondition = false;

        public int Priority { get; set; }
        public bool IsInterruptible { get; set; }

        public List<Transition> Transitions { get; } = new();

        // should be injected
        public VoidChannelSo OnEnter { private get; set; }
        public VoidChannelSo OnExecute { private get; set; }
        public VoidChannelSo OnExit { private get; set; }

        public VoidChannelSo OnEnterCondition { private get; set; }
        public VoidChannelSo OnExitCondition { private get; set; }


        public sealed override void Enter()
        {
            if (OnEnter != null) OnEnter.TriggerEvent();
            if (OnEnterCondition != null) OnEnterCondition.Subscribe(VerifyEnterCondition);
            if (OnExitCondition != null) OnExitCondition.Subscribe(VerifyExitCondition);

            EnterLogic();
        }

        public sealed override void Execute(IStateMachine stateMachine, float deltaTime)
        {
            ExecuteLogic(deltaTime);

            // Checking transitions.

            if (!ExitCondition() && !IsInterruptible) return;

            for (int i = 0; i < Transitions.Count; i++)
            {
                var transition = Transitions[i];

                if (stateMachine.IsSameState(transition.NextState, this)) continue;
                if (!transition.ConditionMet()) continue;

                stateMachine.TransitionTo(transition.NextState);

                break;
            }
        }

        public sealed override void Exit()
        {
            ExitLogic();

            if (OnExit != null) OnExit.TriggerEvent();
            if (OnEnterCondition != null) OnEnterCondition.Unsubscribe(VerifyEnterCondition);
            if (OnExitCondition != null) OnExitCondition.Unsubscribe(VerifyExitCondition);
        }

        protected virtual void EnterLogic() { }
        protected virtual void ExecuteLogic(float deltaTime) { }
        protected virtual void ExitLogic() { }

        public virtual bool EnterCondition()
        {
            if (!enterCondition) return false;

            enterCondition = false;
            return true;
        }

        protected virtual bool ExitCondition()
        {
            if (!exitCondition) return false;

            exitCondition = false;
            return true;
        }

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

        private void VerifyEnterCondition() => enterCondition = true;
        private void VerifyExitCondition() => exitCondition = true;
    }
}
