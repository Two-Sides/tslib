using TwoSides.AI.Behaviour.StateMachines;

public class HierarchicalState : State
{
    public SubStateMachine Ancestor { get; set; }
}