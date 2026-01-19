using UnityEngine;

namespace TwoSides.Utility.Management.Component.Capabilities
{
    public abstract class ComponentBase : MonoBehaviour, IComponent
    {
        public virtual void Initialize() { }
        public virtual void Configure() { }
        public virtual void Deconfigure() { }
    }
}
