namespace TwoSides.AI.Behaviour.StateMachines
{
    public interface IStateMachine
    {
        public void Execute();

        public void TransitionTo(State newState, bool allowSameState = false);

        public void RevertToPrevious(bool allowSameState = false);

        public bool IsSameState(State s1, State s2);
    }
}