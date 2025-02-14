using UnityEngine;
using System.Reflection;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static MonoSingletonFlags? singletonFlag;
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                if (IsShuttingDown) return null;

                _instance = RuntimeInitialize();
            }
            return _instance;
        }
    }
    private static bool IsShuttingDown { get; set; }
    private static T RuntimeInitialize()
    {
        GameObject gameObject = new(name: "Runtime_Singleton_" + typeof(T).Name);
        T result = gameObject.AddComponent<T>();

        Debug.Log("Runtime_Singleton_" + typeof(T).Name);
        return result;
    }
    protected virtual void Awake()
    {
        //check two singleton error
        if (_instance is not null)
        {
            Debug.LogError("[ERROR]TwoSingletons_" + typeof(T).Name);
            Destroy(gameObject);
            return;
        }

        //custom singleton attribute setting
        if (!singletonFlag.HasValue)
        {
            var singletonAttribute = typeof(T).GetCustomAttribute<MonoSingletonUsageAttribute>();
            singletonFlag = singletonAttribute != null
                ? singletonAttribute.Flag
                : MonoSingletonFlags.None;
            if (singletonFlag.Value.HasFlag(MonoSingletonFlags.DontDestroyOnLoad)) DontDestroyOnLoad(gameObject);
        }

        //init
#if UNITY_EDITOR
        print($"[Singleton_Awake] [type : {typeof(T).Name}] [name : {gameObject.name}]");
#endif
        _instance = this as T;

    }
    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
    protected virtual void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }
}