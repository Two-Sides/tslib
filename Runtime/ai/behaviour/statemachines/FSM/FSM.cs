using System;
using System.Collections.Generic;

namespace TwoSides.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Generic finite state machine (FSM) implementation that manages state transitions.
    /// </summary>
    public class FSM : IStateMachine
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
        /// Creates a new finite state machine.
        /// </summary>
        /// <param name="currentState">The initial active state.</param>
        /// <param name="previousState">
        /// The initial previous state. If <c>null</c>, it defaults to <paramref name="currentState"/>.
        /// </param>
        /// <param name="stateComparer">
        /// Optional comparer used to determine state equality.
        /// If <c>null</c>, <see cref="EqualityComparer{T}.Default"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="currentState"/> is <c>null</c>.
        /// </exception>
        public FSM(
            State currentState,
            State previousState = null,
            IEqualityComparer<State> stateComparer = null
            )
        {
            CurrentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
            PreviousState = previousState ?? currentState;
            StateComparer = stateComparer ?? EqualityComparer<State>.Default;

            // Enters the initial state immediately upon creation.
            CurrentState.Enter();
        }

        /// <summary>
        /// The current state is updated.
        /// </summary>
        public void Execute()
        {
            CurrentState?.Execute(this);
        }

        /// <summary>
        /// Transitions the FSM to a new current state.
        /// </summary>
        /// <param name="newState">The state to transition to.</param>
        /// <param name="doEnter">
        /// If <c>true</c>, executes <see cref="State.Enter"/> while transitioning.
        /// </param>
        /// <param name="doExit">
        /// If <c>true</c>, executes <see cref="State.Exit"/> while transitioning.
        /// </param>
        /// <param name="allowSameState">
        /// If <c>true</c>, allows re-entering the same state even if it is considered equal
        /// to the current state.
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
        /// Reverts the FSM to the previously active state.
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

