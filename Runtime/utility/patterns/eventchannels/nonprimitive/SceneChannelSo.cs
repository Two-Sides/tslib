using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "SceneChannelSo",
        menuName = "EventChannels/Actions/SceneChannelSo"
    )]
    public class SceneChannelSo : SingleActionChannelBaseSo<UnityEngine.SceneManagement.Scene> { }
}

