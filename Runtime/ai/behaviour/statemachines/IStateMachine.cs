namespace TwoSides.AI.Behaviour.StateMachines
{
    public interface IStateMachine<TEntity>
    {
        public void Update();

        public void ChangeState(State<TEntity> newState, bool allowSameState = false);

        public void RevertToPreviousState(bool allowSameState = false);

        public void ChangeOwner(TEntity newOwner, bool reenterCurrentState);

        public bool IsSameState(State<TEntity> s1, State<TEntity> s2);
    }
}