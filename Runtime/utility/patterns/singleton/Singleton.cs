using TSLib.Utility.Management.Component.Capabilities;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour, IInitializable where T : MonoBehaviour
{
    protected virtual bool Persistent => true;
    protected static T instance;
    private static bool _isQuitting;

    public static T Instance
    {
        get
        {
            if (_isQuitting) return null;

            if (instance != null) return instance;

            instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);

            return instance;
        }
    }

    public void Initialize()
    {
        if (!Application.isPlaying) return;

        if (instance == null)
        {
            instance = this as T;

            if (Persistent) DontDestroyOnLoad(gameObject);

            OnSingletonAwake();
            return;
        }

        if (instance != this) Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (instance != this) return;
        instance = null;
    }

    protected virtual void OnApplicationQuit() => _isQuitting = true;

    protected virtual void OnSingletonAwake() { }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        instance = null;
        _isQuitting = false;
    }
}
