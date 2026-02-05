using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "SceneChannel",
        menuName = "Event Channels/NonPrimitive/Scene Channel"
    )]
    public class SceneChannel_So : ChannelBaseT1_So<UnityEngine.SceneManagement.Scene> { }
}

