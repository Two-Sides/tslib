using System;
using System.Collections.Generic;
using TSLib.AI.Behaviour.StateMachines.HFSM;
using TSLib.Utility.Debug.Logging;
using TSLib.Utility.Patterns.EventChannels.Primitive;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public abstract class PHS : HierarchicalState
    {
        public bool EnterCondition { get; private set; } = false;
        public bool ExitCondition { get; private set; } = false;

        public int Priority { get; set; }
        public bool IsInterruptible { get; set; }

        public List<Transition> Transitions { get; private set; }
        public Transition SelfTransition { get; private set; }

        public VoidChannel_So OnEnter { private get; set; }
        public VoidChannel_So OnExecute { private get; set; }
        public VoidChannel_So OnExit { private get; set; }

        public VoidChannel_So OnEnterCondition { private get; set; }
        public VoidChannel_So OnExitCondition { private get; set; }

        private readonly IComparer<Transition> _comparer;


        protected PHS(PHSData_So data, IComparer<Transition> comparer)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));


            Priority = data.Priority;
            IsInterruptible = data.IsInterruptible;

            OnEnter = data.OnEnter ? data.OnEnter : null;
            OnExecute = data.OnExecute ? data.OnExecute : null;
            OnExit = data.OnExit ? data.OnExit : null;

            OnEnterCondition = data.OnEnterCondition ? data.OnEnterCondition : null;
            OnExitCondition = data.OnExitCondition ? data.OnExitCondition : null;

            SelfTransition = new Transition(this);
            _comparer = comparer;

            if (OnEnterCondition != null) OnEnterCondition.Subscribe(EnableEnter);
            if (OnExitCondition != null) OnExitCondition.Subscribe(EnableExit);
        }


        public sealed override void Enter()
        {
            EnterCondition = false;

            if (OnEnter != null) OnEnter.TriggerEvent();

            EnterLogic();
        }

        public sealed override void Execute(IStateMachine stateMachine, float deltaTime)
        {
            ExecuteLogic(deltaTime);

            // Transition checker
            if (!ExitCondition && !IsInterruptible) return;

            for (int i = 0; i < Transitions.Count; i++)
            {
                var nextTransition = Transitions[i];
                if (nextTransition == null) continue;

                if (!nextTransition.EnterCondition()) continue;
                if (stateMachine.IsSameState(nextTransition.NextState, this)) continue;

                bool isInterruption = !ExitCondition && IsInterruptible;
                if (isInterruption && stateMachine.IsSameState(GetHighestPriority(SelfTransition, nextTransition), this))
                    break;

                stateMachine.TransitionTo(nextTransition.NextState);

                break;
            }
        }

        public sealed override void Exit()
        {
            ExitCondition = false;

            ExitLogic();

            if (OnExit != null) OnExit.TriggerEvent();
        }

        public void SetTransitions(List<Transition> transitions)
        {
            if (transitions == null) throw new ArgumentNullException(nameof(transitions));
            if (transitions.Count == 0) throw new InvalidOperationException(
                "(empty) no transitions to set.");

            Transitions = transitions;
            Transitions.Sort(_comparer);
        }

        public void UnsubscribeConditionEvents()
        {
            if (OnEnterCondition != null) OnEnterCondition.Unsubscribe(EnableEnter);
            if (OnExitCondition != null) OnExitCondition.Unsubscribe(EnableExit);
        }

        protected virtual void EnterLogic() { }
        protected virtual void ExecuteLogic(float deltaTime) { }
        protected virtual void ExitLogic() { }

        protected virtual void EnableEnter() => EnterCondition = true;
        protected virtual void EnableExit() => ExitCondition = true;
        protected virtual void DisableEnter() => EnterCondition = false;
        protected virtual void DisableExit() => ExitCondition = false;

        private PHS GetHighestPriority(Transition t1, Transition t2)
        {
            if (_comparer.Compare(t1, t2) <= 0) return t1.NextState;
            return t2.NextState;
        }
    }
}
