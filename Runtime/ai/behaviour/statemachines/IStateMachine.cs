namespace TwoSides.AI.Behaviour.StateMachines
{
    public interface IStateMachine
    {
        public void Update();

        public void ChangeState(State newState, bool allowSameState = false);

        public void RevertToPreviousState(bool allowSameState = false);

        public bool IsSameState(State s1, State s2);
    }
}