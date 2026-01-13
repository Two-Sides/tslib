using System.Collections.Generic;
using TwoSides.AI.Behaviour.StateMachines;
using UnityEngine;

public class HFSM
{
    public State CurrentLeaf { get; set; }

    public HFSM(
        State initialState
    )
    {

    }
}

public class Prueba : MonoBehaviour
{
    void Start()
    {
        SubStateMachine root = new(null);

        SubStateMachine A = new(root);
        SubStateMachine B = new(root);

        C C = new() { Ancestor = A };
        D D = new() { Ancestor = A };
        E E = new() { Ancestor = B };
        F F = new() { Ancestor = B };
    }
}

public class C : HierarchicalState { }
public class D : HierarchicalState { }
public class E : HierarchicalState { }
public class F : HierarchicalState { }