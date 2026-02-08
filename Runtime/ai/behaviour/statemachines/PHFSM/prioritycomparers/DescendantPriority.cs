using System.Collections.Generic;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM.PriorityComparers
{
    public sealed class DescendantPriority : IComparer<Transition>
    {
        public int Compare(Transition x, Transition y)
        {
            if (x == null || x.NextState == null) return 1;
            if (y == null || y.NextState == null) return -1;

            int xp = x.NextState.Priority;
            int yp = y.NextState.Priority;

            return yp.CompareTo(xp);
        }
    }
}
