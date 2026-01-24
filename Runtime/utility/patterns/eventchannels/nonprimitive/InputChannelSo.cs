using UnityEngine;
using UnityEngine.InputSystem;

namespace TSLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "InputChannelSo",
        menuName = "EventChannels/Actions/InputChannelSo"
    )]
    public class InputChannelSo : SingleActionChannelBaseSo<InputAction.CallbackContext> { }

}


