using UnityEngine;

public abstract class ServiceBaseSo : ScriptableObject
{
    public virtual void Register()
    {
        ServiceLocator.Register(this);
    }
}
