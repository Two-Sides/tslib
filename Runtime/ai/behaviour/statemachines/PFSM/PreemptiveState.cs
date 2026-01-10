namespace TwoSides.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Represents a preemptive state that can interrupt the current active state
    /// of the PFSM and take control when its preemption conditions are met.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity controlled by the state machine.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// A <see cref="PreemptiveState{TEntity}"/> is both a decision maker and a regular state.
    /// While it is not the current active state, its
    /// <see cref="EvaluatePreemption(PFSM{TEntity}, TEntity)"/> method is evaluated every update
    /// cycle to determine whether a transition into this state (or another state) should occur.
    /// </para>
    /// <para>
    /// When a preemptive state becomes the current active state, it fully participates in the
    /// normal state lifecycle: its <see cref="State{TEntity}.Enter"/>,
    /// <see cref="State{TEntity}.Execute"/>, and <see cref="State{TEntity}.Exit"/> methods are
    /// invoked just like any other state.
    /// </para>
    /// <para>
    /// This pattern is intended for high-priority or interrupting behaviors that must be able
    /// to take control from any other state, such as forced actions, emergency states, or
    /// external interruptions.
    /// </para>
    /// </remarks>
    public abstract class PreemptiveState<TEntity> : State<TEntity>
    {
        /// <summary>
        /// Evaluates whether this state should preempt the currently active state.
        /// </summary>
        /// <param name="fsm">The finite state machine evaluating the preemption.</param>
        /// <param name="entity">The entity controlled by the state machine.</param>
        public abstract void EvaluatePreemption(PFSM<TEntity> pfsm, TEntity entity);
    }
}