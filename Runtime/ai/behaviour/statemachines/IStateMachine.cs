namespace TwoSides.AI.Behaviour.StateMachines
{
    public interface IStateMachine
    {
        public void Execute();

        public void TransitionTo(State newState, bool doEnter, bool doExit, bool allowSameState);

        public void RevertToPrevious(bool doEnter, bool doExit, bool allowSameState);

        public bool IsSameState(State s1, State s2);
    }
}