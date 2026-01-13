namespace TwoSides.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Represents a preemptive state that can interrupt the current active state
    /// of the PFSM and take control when its preemption conditions are met.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="PreemptiveState"/> is both a decision maker and a regular state.
    /// While it is not the current active state, its
    /// <see cref="EvaluatePreemption(PFSM)"/> method is evaluated every update
    /// cycle to determine whether a transition into this state (or another state) should occur.
    /// </para>
    /// <para>
    /// When a preemptive state becomes the current active state, it fully participates in the
    /// normal state lifecycle: its <see cref="State.Enter"/>,
    /// <see cref="State.Execute"/>, and <see cref="State.Exit"/> methods are
    /// invoked just like any other state.
    /// </para>
    /// <para>
    /// This pattern is intended for high-priority or interrupting behaviors that must be able
    /// to take control from any other state, such as forced actions, emergency states, or
    /// external interruptions.
    /// </para>
    /// </remarks>
    public abstract class PreemptiveState : State
    {
        /// <summary>
        /// Evaluates whether this state should preempt the currently active state.
        /// </summary>
        /// <param name="pfsm">The finite state machine evaluating the preemption.</param>
        public abstract void EvaluatePreemption(PFSM pfsm);
    }
}