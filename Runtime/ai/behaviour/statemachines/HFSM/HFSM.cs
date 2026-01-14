using System;
using System.Collections.Generic;
using TwoSides.AI.Behaviour.StateMachines;

public class HFSM : IStateMachine
{
    public bool Started { get; set; }
    public bool Running { get; set; }
    public HierarchicalState Root { get; set; }
    public HierarchicalState CurrentLeaf { get; set; } // lower active state
    public HierarchicalState PreviousLeaf { get; set; } // previous lower active state
    public List<HierarchicalState> CurrentPath { get; set; } // reversed active path
    public IEqualityComparer<State> StateComparer { get; private set; }

    public HFSM(
        HierarchicalState root,
        HierarchicalState initial,
        int maxDepth,
        IEqualityComparer<State> stateComparer = null)
    {
        Started = false;
        Running = false;

        CurrentPath = new(maxDepth);
        SetCurrentPath(initial);

        Root = root;
        CurrentLeaf = initial;
        PreviousLeaf = null;
        StateComparer = stateComparer ?? EqualityComparer<State>.Default;
    }

    private void SetCurrentPath(HierarchicalState leaf)
    {
        if (CurrentPath == null)
            throw new ArgumentNullException(nameof(CurrentPath));

        var state = leaf;
        CurrentPath.Clear();

        while (state != Root)
        {
            CurrentPath.Add(state); // added from bottom to root
            state = state.Ancestor; // next node in path
        }
    }

    public void Start()
    {
        if (Running)
            throw new InvalidOperationException(
                $"(running) The HFSM must be stopped before starting it again."
            );

        for (int i = CurrentPath.Count - 1; i >= 0; i--)
        {
            var state = CurrentPath[i];
            state?.Enter(this);
        }

        Started = true;
    }

    public void Run()
    {
        if (!Started)
            throw new InvalidOperationException(
                $"(not started) Before running the HFSM must be started."
            );

        Running = true;
    }

    public void Stop()
    {
        Started = false;
        Running = false;
    }

    public void ChangeState(State newLeaf, bool allowSameState = false)
    {
        if (newLeaf == null)
            throw new ArgumentNullException(nameof(newLeaf));

        if (newLeaf is not HierarchicalState targetLeaf)
            throw new InvalidCastException(
                $"Expected HierarchicalState but received {newLeaf.GetType().Name}."
            );

        if (!allowSameState && IsSameState(CurrentLeaf, targetLeaf))
            throw new InvalidOperationException(
                $"(same current state) The new state cannot be equals to the current one."
            );

        Stop(); // stops the HFSM

        // save current leaf
        PreviousLeaf = CurrentLeaf;

        // get the lowest common node
        var lca = LCA(CurrentLeaf, targetLeaf);

        // exit from bottom to lca
        while (CurrentLeaf != lca)
        {
            CurrentLeaf?.Exit(this);
            CurrentLeaf = CurrentLeaf.Ancestor;
        }

        SetCurrentPath(targetLeaf); // updates current path

        CurrentLeaf = targetLeaf; // updates current leaf.

        Start(); // starts again the machine
        Run(); // continues running but using the new path
    }

    public HierarchicalState LCA(HierarchicalState node1, HierarchicalState node2)
    {
        int depthNode1 = Depth(node1);
        int depthNode2 = Depth(node2);

        while (depthNode1 > depthNode2)
        {
            node1 = node1.Ancestor;
            depthNode1--;
        }

        while (depthNode2 > depthNode1)
        {
            node2 = node2.Ancestor;
            depthNode2--;
        }

        while (node1 != node2)
        {
            node1 = node1.Ancestor;
            node2 = node2.Ancestor;
        }

        return node1; // node1 = node2
    }

    public int Depth(HierarchicalState node)
    {
        int d = 0;

        while (node.Ancestor != null)
        {
            node = node.Ancestor;
            d++;
        }

        return d;
    }

    public void Update()
    {
        if (!Running || !Started) return;

        for (int i = CurrentPath.Count - 1; i >= 0; i--)
        {
            var state = CurrentPath[i];
            state?.Execute(this);
        }
    }

    public void RevertToPreviousState(bool allowSameState = false)
    {
        if (PreviousLeaf == null)
            throw new ArgumentNullException(nameof(PreviousLeaf));

        ChangeState(PreviousLeaf, allowSameState);
    }

    public bool IsSameState(State s1, State s2)
    {
        return StateComparer.Equals(s1, s2);
    }
}