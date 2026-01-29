using TSLib.AI.Behaviour.StateMachines.PHFSM;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

[CreateAssetMenu(
    fileName = "StateDataName",
    menuName = "ScriptableObjects/HFSM/StateDataName"
)]
public class PriorityHierarchicalStateDataSo : ScriptableObject
{
    [field: SerializeField] public int Priority { get; private set; }
    [field: SerializeField] public bool IsInterruptible { get; private set; }

    [field: SerializeField] public VoidChannelSo OnEnter { get; private set; }
    [field: SerializeField] public VoidChannelSo OnExecute { get; private set; }
    [field: SerializeField] public VoidChannelSo OnExit { get; private set; }

    [field: SerializeField] public VoidChannelSo OnEnterCondition { get; private set; }
    [field: SerializeField] public VoidChannelSo OnExitCondition { get; private set; }

    public void SetData(PriorityHierarchicalState state)
    {
        state.Priority = Priority;
        state.IsInterruptible = IsInterruptible;
        state.OnEnter = OnEnter ? OnEnter : null;
        state.OnExecute = OnExecute ? OnExecute : null;
        state.OnExit = OnExit ? OnExit : null;
        state.OnEnterCondition = OnEnterCondition ? OnEnterCondition : null;
        state.OnExitCondition = OnExitCondition ? OnExitCondition : null;
    }
}