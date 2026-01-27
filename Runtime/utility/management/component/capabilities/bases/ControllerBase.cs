using System;
using UnityEngine;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public abstract class ControllerBase : ComponentBase
    {
        [SerializeField] protected ComponentBase[] Components;

        public virtual void Register() { }
        public virtual void Unregister() { }

        public override void Initialize()
        {
            if (Components == null)
                throw new ArgumentNullException(nameof(Components));

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Initialize();
            }
        }

        public override void Configure()
        {
            if (Components == null)
                throw new ArgumentNullException(nameof(Components));

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Configure();
            }
        }

        public override void Deconfigure()
        {
            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;

                component.Deconfigure();
                Components[i] = null;
            }

            Components = null;
        }

        protected virtual void OnEnable()
        {
            if (Components == null)
                throw new ArgumentNullException(nameof(Components));

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Activate();
            }
        }

        protected virtual void OnDisable()
        {
            if (Components == null)
                throw new ArgumentNullException(nameof(Components));

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Deactivate();
            }
        }

    }
}