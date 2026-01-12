using UnityEngine;

namespace TwoSides.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "SceneChannelSo",
        menuName = "EventChannels/Actions/SceneChannelSo"
    )]
    public class SceneChannelSo : ActionChannelBaseSo<UnityEngine.SceneManagement.Scene> { }
}

