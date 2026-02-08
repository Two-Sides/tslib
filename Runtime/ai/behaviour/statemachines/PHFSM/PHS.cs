using System;
using System.Collections.Generic;
using TSLib.AI.Behaviour.StateMachines.HFSM;
using TSLib.Utility.Debug.Logging;
using TSLib.Utility.Patterns.EventChannels.Primitive;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public abstract class PHS : HierarchicalState
    {
        public int Priority { get; set; }
        public bool IsInterruptible { private get; set; }

        public List<Transition> TransitionList { get; private set; }
        public Transition SelfTransition { get; private set; }

        public VoidChannel_So OnEnter { private get; set; }
        public VoidChannel_So OnExecute { private get; set; }
        public VoidChannel_So OnExit { private get; set; }

        public bool EnterCondition { get; private set; } = false;
        public bool ExitCondition { get; private set; } = false;
        public VoidChannel_So OnEnterCondition { private get; set; }
        public VoidChannel_So OnExitCondition { private get; set; }

        private readonly IComparer<Transition> _comparer;


        protected PHS(PHSData_So data, IComparer<Transition> comparer)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Priority = data.Priority;
            IsInterruptible = data.IsInterruptible;

            OnEnter = data.OnEnter ? data.OnEnter : null;
            OnExecute = data.OnExecute ? data.OnExecute : null;
            OnExit = data.OnExit ? data.OnExit : null;

            OnEnterCondition = data.OnEnterCondition ? data.OnEnterCondition : null;
            OnExitCondition = data.OnExitCondition ? data.OnExitCondition : null;

            SelfTransition = new Transition(this);
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

            if (OnEnterCondition != null) OnEnterCondition.Subscribe(EnableEnter);
            if (OnExitCondition != null) OnExitCondition.Subscribe(EnableExit);
        }


        public sealed override void Enter()
        {
            EnterCondition = false;
            if (OnEnter != null) OnEnter.TriggerEvent();
            EnterLogic();
        }

        public sealed override void Execute(StateMachineBase stateMachine, float deltaTime)
        {
            ExecuteLogic(stateMachine, deltaTime);

            // Transition checker
            if (!ExitCondition && !IsInterruptible) return;

            for (int i = 0; i < TransitionList.Count; i++)
            {
                var nextTransition = TransitionList[i];
                if (nextTransition == null) continue;

                if (!nextTransition.EnterCondition()) continue;
                if (stateMachine.IsSameState(nextTransition.NextState, this)) continue;

                // can transition to a state with higher priority
                bool isInterruption = !ExitCondition && IsInterruptible;
                bool isHigherPriorityState = !stateMachine.IsSameState(GetHigherPriorityState(SelfTransition, nextTransition), this);

                if (isInterruption && !isHigherPriorityState) break;

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

        public void SetTransitionList(List<Transition> transitionList)
        {
            if (transitionList == null) throw new ArgumentNullException(nameof(transitionList));
            if (transitionList.Count == 0) throw new InvalidOperationException("(empty) no transitions to set.");

            TransitionList = transitionList;
            TransitionList.Sort(_comparer);
        }

        public void UnsubscribeConditionEvents()
        {
            if (OnEnterCondition != null) OnEnterCondition.Unsubscribe(EnableEnter);
            if (OnExitCondition != null) OnExitCondition.Unsubscribe(EnableExit);
        }

        protected virtual void EnterLogic() { }
        protected virtual void ExecuteLogic(StateMachineBase stateMachine, float deltaTime) { }
        protected virtual void ExitLogic() { }

        protected virtual void EnableEnter() => EnterCondition = true;
        protected virtual void EnableExit() => ExitCondition = true;
        protected virtual void DisableEnter() => EnterCondition = false;
        protected virtual void DisableExit() => ExitCondition = false;

        private PHS GetHigherPriorityState(Transition t1, Transition t2)
        {
            return _comparer.Compare(t1, t2) <= 0 ? t1.NextState : t2.NextState;
        }
    }
}
