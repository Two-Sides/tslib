using UnityEngine;

namespace TwoSides.Utility.Management.Service
{
    /// <summary>
    /// Base class for services implemented as <see cref="ScriptableObject"/>s.
    ///
    /// Services derived from this class are intended to be registered in a
    /// global service locator during application or scene initialization.
    ///
    /// The default implementation registers the service instance using
    /// <see cref="ServiceLocator"/>, but derived classes may override
    /// <see cref="Register"/> to provide custom registration behavior.
    /// </summary>
    public abstract class ServiceBaseSo : ScriptableObject
    {
        /// <summary>
        /// Registers this service instance in the global service locator.
        ///
        /// Override this method to customize how or where the service is
        /// registered.
        /// </summary>
        public virtual void Register()
        {
            ServiceLocator.Register(this);
        }
    }
}


