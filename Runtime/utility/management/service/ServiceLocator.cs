using System;
using System.Collections.Generic;

namespace TSLib.Utility.Management.Service
{
    /// <summary>
    /// Simple static service locator used to register and retrieve globally
    /// accessible services by their concrete type.
    ///
    /// This locator stores instances derived from <see cref="ServiceBaseSo"/>
    /// and allows systems to query them without requiring direct references.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Internal registry mapping service concrete types to their instances.
        /// </summary>
        private static readonly Dictionary<Type, ServiceBaseSo> _services = new();

        /// <summary>
        /// Indicates whether the ServiceLocator is enabled.
        /// When this value is true, the use of the ServiceLocator is allowed.
        /// </summary>
        public static bool Active { get; private set; } = false;

        /// <summary>
        /// Sets the activation state of the ServiceLocator.
        /// </summary>
        /// <param name="active">
        /// Determines the ServiceLocator state:
        /// true enables it, false disables it.
        /// </param>
        public static void SetActive(bool active) => Active = active;

        /// <summary>
        /// Registers a service instance using its concrete runtime type as the key.
        /// Duplicate service registration is not allowed.
        /// </summary>
        /// <typeparam name="T">Concrete service type.</typeparam>
        /// <param name="service">Service instance to register.</param>
        public static void Register<T>(T service) where T : ServiceBaseSo
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) The ServiceLocator is currently disabled. Call SetActive(true) before attempting to use it.");

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var type = service.GetType();

            if (_services.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"A service of type '{type.FullName}' is already registered. " +
                    "Duplicate service registration is not allowed."
                );
            }

            _services[type] = service;
        }

        /// <summary>
        /// Unregisters a previously registered service of the specified type.
        /// If no service of that type is registered, the call has no effect.
        /// </summary>
        public static void Unregister<T>() where T : ServiceBaseSo
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) The ServiceLocator is currently disabled. Call SetActive(true) before attempting to use it.");

            _services.Remove(typeof(T));
        }

        /// <summary>
        /// Retrieves a registered service of the specified type.
        /// </summary>
        /// <typeparam name="T">Concrete service type.</typeparam>
        /// <returns>The registered service instance, or <c>null</c> if not found.</returns>
        public static T Get<T>() where T : ServiceBaseSo
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) The ServiceLocator is currently disabled. Call SetActive(true) before attempting to use it.");

            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
                return service as T;

            return default;
        }

        /// <summary>
        /// Clears all registered services.
        /// Intended to be called when unloading a scene or resetting application state.
        /// </summary>
        public static void Clear()
        {
            if (!Active) throw new InvalidOperationException(
                "(disabled) The ServiceLocator is currently disabled. Call SetActive(true) before attempting to use it.");

            _services.Clear();
        }
    }
}
