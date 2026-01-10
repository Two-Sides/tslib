using System.Collections.Generic;
using UnityEngine;

public class ServiceInstaller : MonoBehaviour
{
    [SerializeField]
    private List<ServiceBaseSo> _services;


    public void Install()
    {
        for (int i = 0; i < _services.Count; i++)
        {
            var service = _services[i];
            if (service == null) continue;
            service.Register();
        }
    }
}
