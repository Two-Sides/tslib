namespace TwoSides.AI.Behaviour.StateMachines
{
    /// <summary>
    /// Base class for all states used by a finite state machine (FSM).
    /// A state encapsulates behavior that is executed while it is active.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Called once when the FSM enters this state.
        /// Use this method to initialize state-specific data,
        /// register listeners, prepare behavior, etc.
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// Called every update cycle while this state is the current active state.
        /// Contains the main behavior logic of the state.
        /// </summary>
        /// <param name="stateMachine">Reference to the owner state machine of this state.</param>
        public virtual void Execute(IStateMachine stateMachine) { }

        /// <summary>
        /// Called once when the FSM exits this state.
        /// Use this method to clean up state-specific data,
        /// unregister listeners,  stop ongoing behavior, etc.
        /// </summary>
        public virtual void Exit() { }
    }
}

