using System;
using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels.Primitive
{
    [CreateAssetMenu(
        fileName = "VoidChannel",
        menuName = "Event Channels/Primitive/Void Channel"
    )]
    public class VoidChannel_So : ScriptableObject
    {
        /// <summary>
        /// Channel event.
        /// </summary>
        private event Action Event;

        /// <summary>
        /// Invokes the event.
        /// </summary>
        public void TriggerEvent()
        {
            Event?.Invoke();
        }

        /// <summary>
        /// Subscription to the event.
        /// </summary>
        /// <param name="subscriber">Action to subscribe</param>
        public void Subscribe(Action subscriber)
        {
            Event += subscriber;
        }

        /// <summary>
        /// Unsubscribe action from event.
        /// </summary>
        /// <param name="subscriber">Action to unsubscribe</param>
        public void Unsubscribe(Action subscriber)
        {
            Event -= subscriber;
        }
    }
}

