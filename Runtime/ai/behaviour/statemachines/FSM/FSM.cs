using System;
using System.Collections.Generic;

namespace TwoSides.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Generic finite state machine (FSM) implementation that manages state transitions,
    /// and ownership of an entity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity controlled by this state machine.
    /// </typeparam>
    public class FSM<TEntity> : IStateMachine<TEntity>
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
        /// public sealed class StateTypeComparer&lt;TEntity&gt; : IEqualityComparer&lt;State&lt;TEntity&gt;&gt;
        /// {
        ///     public bool Equals(State&lt;TEntity&gt; x, State&lt;TEntity&gt; y)
        ///     {
        ///         if (ReferenceEquals(x, y)) return true;
        ///         if (x is null || y is null) return false;
        ///         return x.GetType() == y.GetType();
        ///     }
        ///
        ///     public int GetHashCode(State&lt;TEntity&gt; obj)
        ///     {
        ///         return obj?.GetType().GetHashCode() ?? 0;
        ///     }
        /// }
        /// </code>
        /// </example>
        public IEqualityComparer<State<TEntity>> StateComparer { get; private set; }

        /// <summary>
        /// The entity currently controlled by this state machine.
        /// </summary>
        public TEntity Owner { get; private set; }

        /// <summary>
        /// The currently active state.
        /// </summary>
        public State<TEntity> CurrentState { get; private set; }

        /// <summary>
        /// The previously active state, typically used for reverting transitions.
        /// </summary>
        public State<TEntity> PreviousState { get; private set; }

        /// <summary>
        /// Creates a new finite state machine.
        /// </summary>
        /// <param name="owner">The entity controlled by the FSM.</param>
        /// <param name="currentState">The initial active state.</param>
        /// <param name="previousState">
        /// The initial previous state. If <c>null</c>, it defaults to <paramref name="currentState"/>.
        /// </param>
        /// <param name="stateComparer">
        /// Optional comparer used to determine state equality.
        /// If <c>null</c>, <see cref="EqualityComparer{T}.Default"/> is used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="owner"/> or <paramref name="currentState"/> is <c>null</c>.
        /// </exception>
        public FSM(
            TEntity owner,
            State<TEntity> currentState,
            State<TEntity> previousState = null,
            IEqualityComparer<State<TEntity>> stateComparer = null
            )
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            CurrentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
            PreviousState = previousState ?? currentState;
            StateComparer = stateComparer ?? EqualityComparer<State<TEntity>>.Default;

            // Enters the initial state immediately upon creation.
            CurrentState.Enter(this, Owner);
        }

        /// <summary>
        /// The current state is updated.
        /// </summary>
        public void Update()
        {
            CurrentState.Execute(this, Owner);
        }

        /// <summary>
        /// Transitions the FSM to a new current state.
        /// </summary>
        /// <param name="newState">The state to transition to.</param>
        /// <param name="allowSameState">
        /// If <c>true</c>, allows re-entering the same state even if it is considered equal
        /// to the current state.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="newState"/> is <c>null</c>.
        /// </exception>
        public void ChangeState(State<TEntity> newState, bool allowSameState = false)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState));

            if (!allowSameState && IsSameState(CurrentState, newState))
                return;

            PreviousState = CurrentState;
            CurrentState.Exit(this, Owner);
            CurrentState = newState;
            CurrentState.Enter(this, Owner);
        }

        /// <summary>
        /// Reverts the FSM to the previously active state.
        /// </summary>
        /// <param name="allowSameState">
        /// If <c>true</c>, allows re-entering the same state if the previous
        /// and current states are considered equal.
        /// </param>
        public void RevertToPreviousState(bool allowSameState = false)
        {
            ChangeState(PreviousState, allowSameState);
        }

        /// <summary>
        /// Changes the owner entity controlled by this FSM.
        /// </summary>
        /// <param name="newOwner">The new owner entity.</param>
        /// <param name="reenterCurrentState">
        /// If <c>true</c>, the current state will be exited and re-entered
        /// using the new owner.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="newOwner"/> is <c>null</c>.
        /// </exception>
        public void ChangeOwner(TEntity newOwner, bool reenterCurrentState)
        {
            if (newOwner == null)
                throw new ArgumentNullException(nameof(newOwner));

            var oldOwner = Owner;
            Owner = newOwner;

            if (reenterCurrentState)
            {
                CurrentState.Exit(this, oldOwner);
                CurrentState.Enter(this, Owner);
            }
        }

        /// <summary>
        /// Determines whether two states are considered the same according to the configured comparer.
        /// </summary>
        /// <param name="s1">The first state.</param>
        /// <param name="s2">The second state.</param>
        /// <returns>
        /// <c>true</c> if the states are considered equal; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSameState(State<TEntity> s1, State<TEntity> s2)
        {
            return StateComparer.Equals(s1, s2);
        }
    }
}

