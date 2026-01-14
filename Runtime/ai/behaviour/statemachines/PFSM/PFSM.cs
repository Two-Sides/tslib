using System;
using System.Collections.Generic;

namespace TwoSides.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Generic preemptive finite state machine (PFSM) implementation that manages state transitions
    /// and preemptive (interrupting) logic.
    /// </summary>
    public class PFSM : IStateMachine
    {
        /// <summary>
        /// Equality comparer used to determine whether two states should be considered the same.
        /// This allows custom comparison logic (by reference, by type, by ID, etc.).
        /// </summary>
        /// <remarks>
        /// By default, <see cref="EqualityComparer{T}.Default"/> is used, which results in
        /// reference-based comparison unless <see cref="object.Equals(object)"/>  is overrided.
        /// </remarks>
        /// <example>
        /// Example of a comparer that considers two states equal if they are of the same runtime type:
        /// <code>
        /// public sealed class StateTypeComparer : IEqualityComparer&lt;State&gt;
        /// {
        ///     public bool Equals(State; x, State y)
        ///     {
        ///         if (ReferenceEquals(x, y)) return true;
        ///         if (x is null || y is null) return false;
        ///         return x.GetType() == y.GetType();
        ///     }
        ///
        ///     public int GetHashCode(State obj)
        ///     {
        ///         return obj?.GetType().GetHashCode() ?? 0;
        ///     }
        /// }
        /// </code>
        /// </example>
        public IEqualityComparer<State> StateComparer { get; private set; }

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
        public PFSM(
            State currentState,
            PreemptiveState preemptiveState,
            State previousState = null,
            IEqualityComparer<State> stateComparer = null
            )
        {
            CurrentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
            PreemptiveState = preemptiveState ?? throw new ArgumentNullException(nameof(preemptiveState));
            PreviousState = previousState ?? currentState;
            StateComparer = stateComparer ?? EqualityComparer<State>.Default;

            // Enters the initial state immediately upon creation.
            CurrentState.Enter();
        }

        /// <summary>
        /// Updates the state machine.
        /// The preemptive state is evaluated first; if it triggers a state change,
        /// the execution of the previous state is skipped for this update cycle.
        /// The current state is updated in second place.
        /// </summary>
        public void Execute()
        {
            var before = CurrentState;

            PreemptiveState?.EvaluatePreemption(this);

            if (!IsSameState(before, CurrentState))
                return; // state changed during preemption; skip Execute for this tick

            CurrentState.Execute(this);
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
        public void TransitionTo(State newState, bool doEnter = true, bool doExit = true, bool allowSameState = false)
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

        /// <summary>
        /// Reverts the PFSM to the previously active state.
        /// </summary>
        /// <param name="allowSameState">
        /// If <c>true</c>, allows re-entering the same state if the previous
        /// and current states are considered equal.
        /// </param>
        /// /// <param name="doEnter">
        /// If <c>true</c>, executes <see cref="State.Enter"/> while transitioning.
        /// </param>
        /// <param name="doExit">
        /// If <c>true</c>, executes <see cref="State.Exit"/> while transitioning.
        /// </param>
        public void RevertToPrevious(bool doEnter = true, bool doExit = true, bool allowSameState = false)
        {
            TransitionTo(PreviousState, doEnter, doExit, allowSameState);
        }

        /// <summary>
        /// Determines whether two states are considered the same according to the configured comparer.
        /// </summary>
        /// <param name="s1">The first state.</param>
        /// <param name="s2">The second state.</param>
        /// <returns>
        /// <c>true</c> if the states are considered equal; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSameState(State s1, State s2)
        {
            return StateComparer.Equals(s1, s2);
        }
    }
}

