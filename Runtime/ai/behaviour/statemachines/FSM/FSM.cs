using System;
using System.Collections.Generic;

namespace TSLib.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Generic finite state machine (FSM) implementation that manages state transitions.
    /// </summary>
    public class FSM : StateMachineBase
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
        public FSM(State currentState, IEqualityComparer<State> stateComparer = null)
        {
            CurrentState = currentState ?? throw new ArgumentNullException(nameof(currentState));
            PreviousState = null;
            StateComparer = stateComparer ?? EqualityComparer<State>.Default;
        }

        /// <summary>
        /// The current state is updated.
        /// </summary>
        public override void Execute(float deltaTime)
        {
            if (!Running || !Started) return;

            CurrentState?.Execute(this, deltaTime);
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

