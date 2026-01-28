using TSLib.Utility.Debug.Logging;
using UnityEngine;

namespace TSLib.Utility.Management.Service
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
        /// Services to be registered during installation.
        /// </summary>
        [SerializeField] private ServiceBaseSo[] _services;

        /// <summary>
        /// Registers all configured services and then destroys this
        /// GameObject once installation completes.
        ///
        /// This method is intended to be called once during application
        /// or scene startup.
        /// </summary>
        public void Install()
        {
            if (_services?.Length == 0)
                TSLogger.LogWarning("(empty) No services registered in Service Installer.");

            for (int i = 0; i < _services.Length; i++)
            {
                var service = _services[i];
                if (service == null) continue;
                service.Install();
            }

            // destroys gameobject after everything is installed.
            Destroy(gameObject);
        }
    }
}


