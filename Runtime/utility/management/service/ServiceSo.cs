using System;
using System.Collections.Generic;
using TwoSides.Utility.Management.Component.Capabilities;
using UnityEngine;

namespace TwoSides.Utility.Management.Service
{
    /// <summary>
    /// ScriptableObject-based service that acts as a registry
    /// for components and a single controller instance.
    /// </summary>
    public abstract class ServiceBaseSo : ScriptableObject
    {
        /// <summary>
        /// Expected maximum number of components to be registered in this service.
        /// This value is used only as a memory preallocation hint and does not impose
        /// any hard limit. Exceeding this number will not cause any issues.
        /// </summary>
        protected abstract int NumberOfComponents { get; }

        // Stores registered components indexed by their concrete type
        private Dictionary<Type, ComponentBase> _registeredComponents;

        // Currently registered controller for this service
        private ControllerBase _registeredController;

        /// <summary>
        /// Install this service instance in the global service locator.
        /// Must be called before registering or accessing components.
        ///
        /// Override this method to customize how or where the service is registered.
        /// </summary>
        public void Install()
        {
            _registeredComponents = new(NumberOfComponents);
            ServiceLocator.Register(this);
        }

        /// Registers a controller for this service.
        /// Only one controller can be registered at a time.
        /// </summary>
        /// <param name="controller">Controller instance to register.</param>
        public void RegisterController(ControllerBase controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            _registeredController = controller;
        }

        /// <summary>
        /// Returns the currently registered controller, or null if none is registered.
        /// </summary>
        public T GetController<T>() where T : ControllerBase => (T)_registeredController;

        /// <summary>
        /// Unregisters the currently registered controller.
        /// </summary>
        public void UnregisterController() => _registeredController = null;

        /// <summary>
        /// Registers a component instance using its concrete runtime type as the key.
        /// If a component of the same type already exists, it is overwritten.
        /// </summary>
        /// <param name="component">Component instance to register.</param>
        public void RegisterComponent(ComponentBase component)
        {
            if (_registeredComponents == null)
                throw new ArgumentNullException(nameof(_registeredComponents));

            if (component == null)
                throw new ArgumentNullException(nameof(component));

            _registeredComponents[component.GetType()] = component;
        }

        /// <summary>
        /// Retrieves a registered component by type.
        /// Returns null if the component is not found or no components are registered.
        /// </summary>
        /// <typeparam name="T">Component type to retrieve.</typeparam>
        public T GetComponent<T>() where T : ComponentBase
        {
            if (_registeredComponents?.Count == 0) return null;

            return _registeredComponents.TryGetValue(typeof(T), out var value)
                ? (T)value : null;
        }

        /// <summary>
        /// Unregisters a component by its type.
        /// </summary>
        /// <typeparam name="T">Component type to unregister.</typeparam>
        /// <returns>True if the component was removed; otherwise false.</returns>
        public bool UnregisterComponent<T>() where T : ComponentBase
        {
            if (_registeredComponents == null)
                throw new ArgumentNullException(nameof(_registeredComponents));

            return _registeredComponents.Remove(typeof(T));
        }

        /// <summary>
        /// Registers multiple components in a single call.
        /// Each component is registered using its concrete type.
        /// </summary>
        /// <param name="components">Array of components to register.</param>
        public void RegisterComponents(ComponentBase[] components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (components.Length == 0)
                throw new InvalidOperationException(
                    "(empty) There is no components registered.");

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];
                RegisterComponent(component);
            }
        }

        /// <summary>
        /// Unregisters multiple components in a single call.
        /// Each component is removed using its concrete type.
        /// </summary>
        /// <param name="components">Array of components to unregister.</param>
        /// <returns>
        /// True if all components were successfully removed;
        /// false if one or more components were not found.
        /// </returns>
        public bool UnregisterComponents(ComponentBase[] components)
        {
            if (_registeredComponents == null)
                throw new ArgumentNullException(nameof(_registeredComponents));

            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (components.Length == 0)
                throw new InvalidOperationException(
                    "(empty) There is no components registered.");

            bool allRemoved = true;

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (component == null)
                    throw new ArgumentNullException(nameof(component));

                bool removed = _registeredComponents.Remove(component.GetType());
                if (!removed) allRemoved = false;
            }

            return allRemoved;
        }
    }
}
