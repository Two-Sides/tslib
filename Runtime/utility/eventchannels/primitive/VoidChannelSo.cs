using System;
using UnityEngine;

namespace TwoSides.Utility.EventChannels.Primitive
{
    [CreateAssetMenu(
        fileName = "VoidChannelSo",
        menuName = "EventChannels/Actions/VoidChannelSo"
    )]
    public class VoidChannelSo : ScriptableObject
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

