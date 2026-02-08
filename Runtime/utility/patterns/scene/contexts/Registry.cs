using System;
using System.Collections.Generic;

namespace TSLib.Utility.Patterns.Scene.Contexts
{
    public class Registry<T>
    {
        public bool Active { get; private set; } = true;

        public void SetActive(bool active) => Active = active;

        private readonly Dictionary<Type, T> _registry = new();

        public void Register(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (_registry == null) throw new InvalidOperationException(
                "(missing) registry storage uninitialized.");

            var type = element.GetType();

            if (_registry.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"(duplicate) An element of type '{type.FullName}' is already registered. " +
                    "Duplicate element registration is not allowed."
                );
            }

            _registry[type] = element;
        }

        public bool Unregister<Type>()
        {
            if (_registry == null) throw new InvalidOperationException(
                "(missing) registry storage uninitialized.");

            return _registry.Remove(typeof(Type));
        }

        public Type Get<Type>() where Type : T
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) the getter is currently disabled.");

            if (_registry == null) throw new InvalidOperationException(
            "(missing) registry storage uninitialized.");

            if (_registry.TryGetValue(typeof(Type), out var element))
                return (Type)element;

            return default;
        }

        public Type Require<Type>() where Type : T
        {
            var element = Get<Type>();

            if (element == null) throw new InvalidOperationException(
                "(missing) element not found.");

            return element;
        }

        public void Clear()
        {
            if (_registry == null) throw new InvalidOperationException(
                "(missing) _registry storage uninitialized.");

            _registry.Clear();
        }
    }
}
