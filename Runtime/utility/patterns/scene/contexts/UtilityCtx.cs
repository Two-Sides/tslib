using System;
using System.Collections.Generic;
using TSLib.Utility.Management.Managers;

namespace TSLib.Utility.Patterns.Scene.Contexts
{
    public sealed class UtilityCtx
    {
        public bool Active { get; private set; } = true;

        public void SetActive(bool active) => Active = active;

        private readonly Dictionary<Type, IUtility> _utilities = new();

        public void Register(IUtility utility)
        {
            if (_utilities == null) throw new InvalidOperationException(
                    "(missing) utilities storage uninitialized.");

            if (utility == null)
                throw new ArgumentNullException(nameof(utility));

            var type = utility.GetType();

            if (_utilities.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"(duplicate) an utility of type '{type.FullName}' is already registered. " +
                    "Duplicate container registration is not allowed."
                );
            }
            _utilities[type] = utility;
        }

        public bool Unregister<T>()
        {
            return _utilities.Remove(typeof(T));
        }

        public T Get<T>() where T : IUtility
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) the getter is currently disabled.");

            if (_utilities == null) throw new InvalidOperationException(
                    "(missing) utilities storage uninitialized.");

            if (_utilities.TryGetValue(typeof(T), out var utility))
                return (T)utility;

            return default;
        }

        public T Require<T>() where T : IUtility
        {
            var utility = Get<T>();

            if (utility == null) throw new InvalidOperationException(
                "(missing) utility not found.");

            return utility;
        }

        public void Clear()
        {
            _utilities.Clear();
        }
    }
}
