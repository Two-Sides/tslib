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
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            _controller = controller;
        }

        public T GetController<T>() where T : ControllerBase => (T)_controller;

        public void UnregisterController() => _controller = null;

        public void RegisterComponent(ComponentBase component)
        {
            if (_components == null)
                throw new ArgumentNullException(nameof(_components));

            if (component == null)
                throw new ArgumentNullException(nameof(component));

            _components[component.GetType()] = component;
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            if (_components?.Count == 0) return null;

            return _components.TryGetValue(typeof(T), out var value)
                ? (T)value : null;
        }

        public bool UnregisterComponent<T>() where T : ComponentBase
        {
            if (_components == null)
                throw new ArgumentNullException(nameof(_components));

            return _components.Remove(typeof(T));
        }

        public void RegisterComponents(ComponentBase[] components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (components.Length == 0) return;

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];
                RegisterComponent(component);
            }
        }

        public bool UnregisterComponents(ComponentBase[] components)
        {
            if (_components == null)
                throw new ArgumentNullException(nameof(_components));

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

                bool removed = _components.Remove(component.GetType());
                if (!removed) allRemoved = false;
            }

            return allRemoved;
        }

    }
}
