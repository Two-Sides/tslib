using System;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public abstract class ControllerBase : ComponentBase, IRegistrable
    {
        [SerializeField] protected ComponentBase[] Components;

        public virtual void Register(SceneCtx sceneCtx, AppCtx appCtx) { }
        public virtual void Unregister(SceneCtx sceneCtx, AppCtx appCtx) { }

        public override void Initialize()
        {
            if (Components == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Initialize();
            }
        }

        public override void Bind(SceneCtx sceneCtx, AppCtx appCtx)
        {
            if (Components == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Bind(sceneCtx, appCtx);
            }
        }

        public override void Configure()
        {
            if (Components == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Configure();
            }
        }

        protected virtual void OnEnable()
        {
            if (Components == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Activate();
            }
        }

        protected virtual void OnDisable()
        {
            if (Components == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < Components.Length; i++)
            {
                var component = Components[i];
                if (component == null) continue;
                component.Deactivate();
            }
        }
    }
}