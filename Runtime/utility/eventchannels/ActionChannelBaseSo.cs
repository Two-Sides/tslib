using System;
using UnityEngine;

namespace TwoSides.Utility.EventChannels
{
    public abstract class ActionChannelBaseSo<T> : ScriptableObject
{
    /// <summary>
    /// Channel event.
    /// </summary>
    private event Action<T> Event;

    /// <summary>
    /// Invokes the event.
    /// </summary>
    /// <param name="param">Generic type for the action</param>
    public void TriggerEvent(T param)
    {
        Event?.Invoke(param);
    }

    /// <summary>
    /// Subscription to the event.
    /// </summary>
    /// <param name="subscriber">Action to subscribe</param>
    public void Subscribe(Action<T> subscriber)
    {
        Event += subscriber;
    }

    /// <summary>
    /// Unsubscribe action from event.
    /// </summary>
    /// <param name="subscriber">Action to unsubscribe</param>
    public void Unsubscribe(Action<T> subscriber)
    {
        Event -= subscriber;
    }
}
}


