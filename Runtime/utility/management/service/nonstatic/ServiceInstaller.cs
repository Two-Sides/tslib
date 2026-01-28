using UnityEngine;

namespace TSLib.Utility.Management.Service.NonStatic
{
    public class ServiceInstaller : MonoBehaviour
    {
        [SerializeField] private ServiceLocatorSo _serviceLocator;

        public void Install()
        {
            _serviceLocator.Install();
            Destroy(gameObject);
        }

    }
}
