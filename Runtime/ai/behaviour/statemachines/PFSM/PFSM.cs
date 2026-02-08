using System;
using System.Collections.Generic;

namespace TSLib.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Generic preemptive finite state machine (PFSM) implementation that manages state transitions
    /// and preemptive (interrupting) logic.
    /// </summary>
    public class PFSM : StateMachineBase
    {
        /// <summary>
        /// The currently active state.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// The previously active state, typically used for reverting transitions.
        /// </summary>
        public State PreviousState { get; private set; }

        /// <summary>
        /// Preemptive state that can evaluate whether it should interrupt the current state.
        /// When a preemption occurs, the preemptive state is transitioned into and behaves like any
        /// other state (its <see cref="State.Enter"/>, <see cref="State.Execute"/>,
        /// and <see cref="State.Exit"/> methods are invoked as part of normal state changes).
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="PreemptiveState.EvaluatePreemption(PFSM)"/> method
        /// is called every update cycle (if a preemptive state is assigned). This method typically decides
        /// whether to call <see cref="TransitionTo(State, bool)"/> to transition into the preemptive
        /// state or another state.
        /// </para>
        /// <para>
        /// This preemptive mechanism is intended for high-priority, interrupting behaviors that may need 
        /// to take over from any other active state.
        /// </para>
        /// </remarks>
        public PreemptiveState PreemptiveState { get; private set; }

        /// <summary>
        /// Creates a new preemptive finite state machine.
        /// </summary>
        /// <param name="currentState">The initial active state.</param>
        /// <param name="previousState">
        /// The initial previous state. If <c>null</c>, it defaults to <paramref name="currentState"/>.
        /// </param>
        /// <param name="preemptiveState">Preemptive state</param>
        /// <param name="stateComparer">
        /// Optional comparer used to determine state equality.
        /// If <c>null</c>, <see cref="EqualityComparer{T}.Default"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="currentState"/> or <paramref name="preemptiveState"/> is <c>null</c>.
        /// </exception>
        public PFSM(State currentState, PreemptiveState preemptiveState,
            IEqualityComparer<State> stateComparer = null)
        {
            CurrentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
            PreemptiveState = preemptiveState ?? throw new ArgumentNullException(nameof(preemptiveState));
            PreviousState = null;
            StateComparer = stateComparer ?? EqualityComparer<State>.Default;
        }

        /// <summary>
        /// Updates the state machine.
        /// The preemptive state is evaluated first; if it triggers a state change,
        /// the execution of the previous state is skipped for this update cycle.
        /// The current state is updated in second place.
        /// </summary>
        public override void Execute(float deltaTime)
        {
            if (!Running || !Started) return;

            var before = CurrentState;

            PreemptiveState?.EvaluatePreemption(this);

            if (!IsSameState(before, CurrentState))
                return; // state changed during preemption; skip Execute for this tick

            CurrentState.Execute(this, deltaTime);
        }

        /// <summary>
        /// Transitions the PFSM to a new current state.
        /// </summary>
        /// <param name="newState">The state to transition to.</param>
        /// <param name="allowSameState">
        /// If <c>true</c>, allows re-entering the same state even if it is considered equal
        /// to the current state.
        /// </param>
        /// /// <param name="doEnter">
        /// If <c>true</c>, executes <see cref="State.Enter"/> while transitioning.
        /// </param>
        /// <param name="doExit">
        /// If <c>true</c>, executes <see cref="State.Exit"/> while transitioning.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="newState"/> is <c>null</c>.
        /// </exception>
        public override void TransitionTo(State newState, bool doEnter = true, bool doExit = true, bool allowSameState = false)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState));

            if (!allowSameState && IsSameState(CurrentState, newState))
                return;

            PreviousState = CurrentState;
            if (doExit) CurrentState.Exit();
            CurrentState = newState;
            if (doEnter) CurrentState.Enter();
        }

        /// <summary>
        /// Sets or replaces the preemptive state.
        /// </summary>
        /// <param name="newPreempState">
        /// The new preemptive state. Can be <c>null</c> to disable preemption.
        /// </param>
        /// <param name="allowSameState">
        /// If <c>true</c>, allows assigning the same preemptive state again.
        /// </param>
        public void SetPreemptiveState(PreemptiveState newPreempState, bool allowSameState = false)
        {
            if (newPreempState == null)
                throw new ArgumentNullException(nameof(newPreempState));

            if (!allowSameState && IsSameState(PreemptiveState, newPreempState))
                return;

            PreemptiveState = newPreempState;
        }

        public override void Start(bool doEnter = true)
        {
            if (Running)
                throw new InvalidOperationException(
                    $"(running) The HFSM must be stopped before starting it again.");

            if (doEnter) CurrentState.Enter();

            Started = true;
        }
    }
}

