using UnityEngine;
using UnityEngine.InputSystem;

namespace TwoSides.Utility.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "InputChannelSo",
        menuName = "EventChannels/Actions/InputChannelSo"
    )]
    public class InputChannelSo : ActionChannelBaseSo<InputAction.CallbackContext> { }

}


