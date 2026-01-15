namespace TwoSides.AI.Behaviour.StateMachines.HFSM
{
    public abstract class HierarchicalState : State
    {
        public HierarchicalState Ancestor { get; set; }
    }
}
