using System;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    [CreateAssetMenu(
    fileName = "PHSData",
    menuName = "Scriptable Objects/HFSM/PHSData"
)]
    public class PHSData_So : ScriptableObject
    {
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public bool IsInterruptible { get; private set; }

        [field: SerializeField] public VoidChannel_So OnEnter { get; private set; }
        [field: SerializeField] public VoidChannel_So OnExecute { get; private set; }
        [field: SerializeField] public VoidChannel_So OnExit { get; private set; }

        [field: SerializeField] public VoidChannel_So OnEnterCondition { get; private set; }
        [field: SerializeField] public VoidChannel_So OnExitCondition { get; private set; }
    }
}
