namespace TwoSides.AI.Behaviour.StateMachines
{
    /// <summary>
    /// Base class for all states used by a finite state machine (FSM).
    /// A state encapsulates behavior that is executed while it is active.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity controlled by the state machine.
    /// </typeparam>
    public abstract class State<TEntity>
    {
        /// <summary>
        /// Called once when the FSM enters this state.
        /// Use this method to initialize state-specific data,
        /// register listeners, prepare behavior, etc.
        /// </summary>
        /// <param name="fsm">The finite state machine that owns this state.</param>
        /// <param name="entity">The entity controlled by the state machine.</param>
        public virtual void Enter(IStateMachine<TEntity> fsm, TEntity entity) { }

        /// <summary>
        /// Called every update cycle while this state is the current active state.
        /// Contains the main behavior logic of the state.
        /// </summary>
        /// <param name="fsm">The finite state machine that owns this state.</param>
        /// <param name="entity">The entity controlled by the state machine.</param>
        public virtual void Execute(IStateMachine<TEntity> fsm, TEntity entity) { }

        /// <summary>
        /// Called once when the FSM exits this state.
        /// Use this method to clean up state-specific data,
        /// unregister listeners,  stop ongoing behavior, etc.
        /// </summary>
        /// <param name="fsm">The finite state machine that owns this state.</param>
        /// <param name="entity">The entity controlled by the state machine.</param>
        public virtual void Exit(IStateMachine<TEntity> fsm, TEntity entity) { }
    }
}

