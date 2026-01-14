using TwoSides.AI.Behaviour.StateMachines;
using UnityEngine;

namespace TwoSides.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "StateChannelSo",
        menuName = "EventChannels/Actions/StateChannelSo"
    )]
    public class StateChannelSo : DoubleActionChannelBaseSo<State, bool> { }
}

