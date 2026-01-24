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
        /// Registers a service instance using its concrete runtime type as the key.
        /// Duplicate service registration is not allowed.
        /// </summary>
        /// <typeparam name="T">Concrete service type.</typeparam>
        /// <param name="service">Service instance to register.</param>
        public static void Register<T>(T service) where T : ServiceBaseSo
        {
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
            _services.Remove(typeof(T));
        }

        /// <summary>
        /// Retrieves a registered service of the specified type.
        /// </summary>
        /// <typeparam name="T">Concrete service type.</typeparam>
        /// <returns>The registered service instance, or <c>null</c> if not found.</returns>
        public static T Get<T>() where T : ServiceBaseSo
        {
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
            _services.Clear();
        }
    }
}
