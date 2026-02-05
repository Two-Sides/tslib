using System;
using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels
{
    public abstract class ChannelBaseT2_So<T1, T2> : ScriptableObject
    {
        /// <summary>
        /// Channel event.
        /// </summary>
        private event Action<T1, T2> Event;

        /// <summary>
        /// Invokes the event.
        /// </summary>
        /// <param name="param1">First generic type for the action</param>
        /// <param name="param2">Second generic type for the action</param>
        public void TriggerEvent(T1 param1, T2 param2)
        {
            Event?.Invoke(param1, param2);
        }

        /// <summary>
        /// Subscription to the event.
        /// </summary>
        /// <param name="subscriber">Action to subscribe</param>
        public void Subscribe(Action<T1, T2> subscriber)
        {
            Event += subscriber;
        }

        /// <summary>
        /// Unsubscribe action from event.
        /// </summary>
        /// <param name="subscriber">Action to unsubscribe</param>
        public void Unsubscribe(Action<T1, T2> subscriber)
        {
            Event -= subscriber;
        }
    }
}


