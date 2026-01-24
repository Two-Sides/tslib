using TSLib.AI.Behaviour.StateMachines;
using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "StateChannelSo",
        menuName = "EventChannels/Actions/StateChannelSo"
    )]
    public class StateChannelSo : DoubleActionChannelBaseSo<State, bool> { }
}

