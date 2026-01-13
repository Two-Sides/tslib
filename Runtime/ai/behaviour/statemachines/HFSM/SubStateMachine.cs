using System.Collections.Generic;
using TwoSides.AI.Behaviour.StateMachines;
using TwoSides.Utility.Patterns.EventChannels.NonPrimitive;
using UnityEngine;

public class SubStateMachine : HierarchicalState, IStateMachine
{

    // Subestados de la máquina.
    public Dictionary<string, HierarchicalState> SubStates { get; set; }

    public HierarchicalState CurrentState { get; set; }
    public HierarchicalState PreviousState { get; set; }

    [SerializeField] private StateChannelSo onChangeState;

    public SubStateMachine(
        SubStateMachine ancestor
        )
    {
        Ancestor = ancestor; // nodo padre
    }

    public void Update()
    {
        CurrentState?.Execute(this);
    }

    public HierarchicalState LCA(HierarchicalState node1, HierarchicalState node2)
    {
        int depthNode1 = Depth(node1);
        int depthNode2 = Depth(node2);

        // Iguala
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

        return node1; // Ambos deben estar en el mismo nodo
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

    // internal substates
    public void ChangeState(State newState, bool allowSameState = false)
    {
        if (newState is not HierarchicalState newHState)
            return;

        PreviousState = CurrentState;

        // lca es el nodo común más bajo.
        var lca = LCA(this, newHState);

        // Hago los exits hacia arriba hasta el LCA (el LCA no hace Exit)
        while (CurrentState != lca)
        {
            CurrentState?.Exit(CurrentState.Ancestor);
            CurrentState = CurrentState.Ancestor;
        }

        // Obtengo la ruta desde el LCA hasta el nuevo nodo.
        var path = new Stack<HierarchicalState>();
        HierarchicalState s = newHState;
        while (s != lca) { path.Push(s); }

        // Bajo en el árbol hasta el nuevo estado llamando a los enter.
        while (path.Count > 0)
        {
            CurrentState = path.Pop();
            CurrentState?.Enter(CurrentState.Ancestor);
        }
    }

    public bool IsSameState(State s1, State s2)
    {
        return false;
    }

    public void RevertToPreviousState(bool allowSameState = false)
    {
        ChangeState(PreviousState, allowSameState);
    }
}