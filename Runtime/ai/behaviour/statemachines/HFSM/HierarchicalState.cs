using TwoSides.AI.Behaviour.StateMachines;

public class HierarchicalState : State
{
    public HierarchicalState Ancestor { get; set; }
}