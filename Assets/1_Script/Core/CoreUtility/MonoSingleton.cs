using UnityEngine;
using System.Reflection;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    //private static class SingletonPresetManager 
    //{
    //    private static T preset = null;
    //    public static T GetPreset => preset;
    //}

    private static MonoSingletonFlags? singletonFlag;
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                if (IsShuttingDown) return null;

                //if (singletonFlag.HasFlag(MonoSingletonFlags.SingletonPreset)) _instance = GetPresetSingleton();
                else _instance = RuntimeInitialize();
            }
            return _instance;
        }
    }
    private static bool IsShuttingDown { get; set; }
    private static T GetPresetSingleton() => default;
    protected virtual void CustomRuntimeInitializeEvent()
    {
    }
    private static T RuntimeInitialize()
    {
        GameObject gameObject = new(name: "Runtime_Singleton_" + typeof(T).Name);
        T result = gameObject.AddComponent<T>();
        if (singletonFlag.Value.HasFlag(MonoSingletonFlags.CustomRuntimeInitialize))
            result.CustomRuntimeInitializeEvent();
        else
            Debug.LogWarning("Runtime_Singleton_" + typeof(T).Name);
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
        print($"[Singleton_Awake] [type : {typeof(T).Name}] [name : {gameObject.name}]");
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