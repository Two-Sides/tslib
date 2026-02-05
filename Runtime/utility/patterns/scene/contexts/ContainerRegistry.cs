using System;
using System.Collections.Generic;

namespace TSLib.Utility.Patterns.Scene.Contexts
{
    public class ContainerRegistry<T>
    {
        public bool Active { get; private set; } = true;

        public void SetActive(bool active) => Active = active;

        private readonly Dictionary<Type, T> _containers = new();

        public void Register(T container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (_containers == null) throw new InvalidOperationException(
                "(missing) containers storage uninitialized.");

            var type = container.GetType();

            if (_containers.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"(duplicate) A container of type '{type.FullName}' is already registered. " +
                    "Duplicate container registration is not allowed."
                );
            }

            _containers[type] = container;
        }

        public bool Unregister<Type>()
        {
            if (_containers == null) throw new InvalidOperationException(
                "(missing) containers storage uninitialized.");

            return _containers.Remove(typeof(Type));
        }

        public Type Get<Type>() where Type : T
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) the getter is currently disabled.");

            if (_containers == null) throw new InvalidOperationException(
            "(missing) containers storage uninitialized.");

            if (_containers.TryGetValue(typeof(Type), out var service))
                return (Type)service;

            return default;
        }

        public Type Require<Type>() where Type : T
        {
            var container = Get<Type>();

            if (container == null) throw new InvalidOperationException(
                "(missing) container not found.");

            return container;
        }

        public void Clear()
        {
            if (_containers == null) throw new InvalidOperationException(
                "(missing) containers storage uninitialized.");

            _containers.Clear();
        }
    }
}
