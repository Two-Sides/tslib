using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, ServiceBaseSo> _services = new();

    public static void Register<T>(T service) where T : ServiceBaseSo
    {
        var type = service.GetType();
        if (!_services.ContainsKey(type))
        {
            _services[type] = service;
        }
    }

    public static void Unregister<T>() where T : ServiceBaseSo
    {
        _services.Remove(typeof(T));
    }

    public static T Get<T>() where T : ServiceBaseSo
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
            return service as T;

        return default;
    }

    public static void Clear()
    {
        _services.Clear();
    }
}
