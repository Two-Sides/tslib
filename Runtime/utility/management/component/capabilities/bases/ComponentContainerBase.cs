using System;
using System.Collections.Generic;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public abstract class ComponentContainerBase
    {
        private readonly Dictionary<Type, ComponentBase> _components = new();

        private ControllerBase _controller;

        public void RegisterController(ControllerBase controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            _controller = controller;
        }

        public T GetController<T>() where T : ControllerBase => (T)_controller;

        public T RequireController<T>() where T : ControllerBase
        {
            var controller = GetController<T>();

            if (controller == null) throw new InvalidOperationException(
                    "(missing) there is no controller registered.");

            return controller;
        }

        public void UnregisterController() => _controller = null;

        public void RegisterComponent(ComponentBase component)
        {
            if (_components == null) throw new InvalidOperationException(
                    "(missing) components storage uninitialized.");

            if (component == null) throw new ArgumentNullException(nameof(component));

            _components[component.GetType()] = component;
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            if (_components == null) throw new InvalidOperationException(
                    "(missing) components storage uninitialized.");

            if (_components.Count == 0) return null;

            return _components.TryGetValue(typeof(T), out var value)
                ? (T)value : null;
        }

        public T RequireComponent<T>() where T : ComponentBase
        {
            var component = GetComponent<T>();

            if (component == null) throw new InvalidOperationException(
                    "(missing) component not found.");

            return component;
        }

        public bool UnregisterComponent<T>() where T : ComponentBase
        {
            if (_components == null) throw new InvalidOperationException(
                    "(missing) components storage uninitialized.");

            return _components.Remove(typeof(T));
        }

        public void RegisterComponents(ComponentBase[] components)
        {
            if (components == null) throw new ArgumentNullException(nameof(components));

            for (int i = 0; i < components.Length; i++)
            {
                RegisterComponent(components[i]);
            }
        }

        public bool UnregisterComponents(ComponentBase[] components)
        {
            if (_components == null) throw new InvalidOperationException(
                    "(missing) components storage uninitialized.");

            if (components == null) throw new ArgumentNullException(nameof(components));

            if (components.Length == 0) throw new InvalidOperationException(
                    "(empty) no components for unregistration.");

            bool allRemoved = true;

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (component == null) continue;

                allRemoved = allRemoved && _components.Remove(component.GetType());
            }
            return allRemoved;
        }


        public void Clear()
        {
            if (_components == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            _components.Clear();
            _controller = null;
        }

    }
}
