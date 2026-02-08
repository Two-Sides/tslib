namespace TSLib.AI.Behaviour.StateMachines
{
    public interface IStateMachine
    {
        public void Execute(float deltaTime);

        public void TransitionTo(State newState, bool doEnter, bool doExit, bool allowSameState);

        public void RevertToPrevious(State previousState, bool doEnter, bool doExit, bool allowSameState);

        public bool IsSameState(State s1, State s2);

        public void Start(bool doEnter);
        public void Run();
        public void Stop();
    }
}