using System.Collections.Generic;
using UnityEngine;

namespace TwoSides.Utility.Workflow.ServiceManagement
{
    /// <summary>
    /// MonoBehaviour responsible for installing a predefined set of services
    /// at runtime.
    ///
    /// This component acts as a one-time bootstrapper: it registers all
    /// referenced <see cref="ServiceBaseSo"/> instances and then destroys
    /// its own GameObject once installation is complete.
    ///
    /// The installed services are expected to live independently of this
    /// component.
    /// </summary>
    public class ServiceInstaller : MonoBehaviour
    {
        /// <summary>
        /// List of services to be registered during installation.
        /// </summary>
        [SerializeField]
        private List<ServiceBaseSo> _services;

        /// <summary>
        /// Registers all configured services and then destroys this
        /// GameObject once installation completes.
        ///
        /// This method is intended to be called once during application
        /// or scene startup.
        /// </summary>
        public void Install()
        {
            for (int i = 0; i < _services.Count; i++)
            {
                var service = _services[i];
                if (service == null) continue;
                service.Register();
            }

            // destroys gameobject after everything is installed.
            Destroy(gameObject);
        }
    }
}


