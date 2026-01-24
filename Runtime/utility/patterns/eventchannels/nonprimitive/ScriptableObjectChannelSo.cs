using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "ScriptableObjectChannelSo",
        menuName = "EventChannels/Actions/ScriptableObjectChannelSo"
    )]
    public class ScriptableObjectChannelSo : SingleActionChannelBaseSo<ScriptableObject> { }
}

