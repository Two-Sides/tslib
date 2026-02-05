using System;
using System.Collections.Generic;
using TSLib.AI.Behaviour.StateMachines.HFSM;
using TSLib.Utility.Patterns.EventChannels.Primitive;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public abstract class PHS : HierarchicalState
    {
        public bool EnterCondition { get; private set; } = false;
        protected bool exitCondition = false;

        public int Priority { get; set; }
        public bool IsInterruptible { get; set; }

        public List<Transition> Transitions { get; }

        public VoidChannel_So OnEnter { private get; set; }
        public VoidChannel_So OnExecute { private get; set; }
        public VoidChannel_So OnExit { private get; set; }

        public VoidChannel_So OnEnterCondition { private get; set; }
        public VoidChannel_So OnExitCondition { private get; set; }

        private IComparer<Transition> _comparer;
        private Transition _selfTransition;

        public PHS(PHSData_So data, Transition selfTransition, IComparer<Transition> comparer, List<Transition> transitions)
        {
            SetData(data, selfTransition, comparer);

            if (transitions == null) throw new ArgumentNullException(nameof(transitions));
            if (transitions.Count == 0) throw new InvalidOperationException(
                "(empty) no transitions to set.");

            Transitions = transitions;
            Transitions.Sort(_comparer);
        }


        public sealed override void Enter()
        {
            EnterCondition = false;

            if (OnEnter != null) OnEnter.TriggerEvent();

            if (OnEnterCondition != null) OnEnterCondition.Subscribe(SetEnterEnabled);
            if (OnExitCondition != null) OnExitCondition.Subscribe(SetExitEnabled);

            EnterLogic();
        }

        public sealed override void Execute(IStateMachine stateMachine, float deltaTime)
        {
            ExecuteLogic(deltaTime);

            // Transition checker
            if (!exitCondition && !IsInterruptible) return;

            for (int i = 0; i < Transitions.Count; i++)
            {
                var nextTransition = Transitions[i];
                if (nextTransition == null) continue;

                if (!nextTransition.EnterCondition()) continue;
                if (stateMachine.IsSameState(nextTransition.NextState, this)) continue;

                bool isInterruption = !exitCondition && IsInterruptible;
                if (isInterruption && _selfTransition != GetHighestPriority(_selfTransition, nextTransition))
                    break;

                stateMachine.TransitionTo(nextTransition.NextState);

                break;
            }
        }

        public sealed override void Exit()
        {
            exitCondition = false;

            ExitLogic();

            if (OnExit != null) OnExit.TriggerEvent();

            if (OnEnterCondition != null) OnEnterCondition.Unsubscribe(SetEnterEnabled);
            if (OnExitCondition != null) OnExitCondition.Unsubscribe(SetExitEnabled);
        }

        protected virtual void EnterLogic() { }
        protected virtual void ExecuteLogic(float deltaTime) { }
        protected virtual void ExitLogic() { }

        private void SetEnterEnabled() => EnterCondition = true;
        private void SetExitEnabled() => exitCondition = true;

        private Transition GetHighestPriority(Transition t1, Transition t2)
        {
            if (_comparer.Compare(t1, t2) <= 0) return t1;
            return t2;
        }

        private void SetData(PHSData_So data, Transition selfTransition, IComparer<Transition> comparer)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (selfTransition == null) throw new ArgumentNullException(nameof(selfTransition));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            Priority = data.Priority;
            IsInterruptible = data.IsInterruptible;

            OnEnter = data.OnEnter ? data.OnEnter : null;
            OnExecute = data.OnExecute ? data.OnExecute : null;
            OnExit = data.OnExit ? data.OnExit : null;

            OnEnterCondition = data.OnEnterCondition ? data.OnEnterCondition : null;
            OnExitCondition = data.OnExitCondition ? data.OnExitCondition : null;

            _selfTransition = selfTransition;
            _comparer = comparer;
        }
    }
}
