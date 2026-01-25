namespace TSLib.AI.Behaviour.StateMachines.HFSM
{
    public abstract class HierarchicalState : State
    {
        public HierarchicalState Ancestor { get; private set; }

        public void SetAncestor(HierarchicalState ancestor)
        {
            Ancestor = ancestor; // can be null
        }
    }
}
