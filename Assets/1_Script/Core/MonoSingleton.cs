using UnityEngine;
using System.Reflection;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    /// <summary>
    /// unfinished
    /// </summary>
    //private static class SingletonPresetManager 
    //{
    //    private static T preset = null;
    //    public static T GetPreset => preset;
    //}

    private static MonoSingletonFlags singletonFlag;
    private static bool IsShuttingDown { get; set; }
    private static T _instance = null;
    
    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                if (IsShuttingDown) return null;
                if (singletonFlag.HasFlag(MonoSingletonFlags.SingletonPreset)) _instance = GetPresetSingleton();
                else _instance = RuntimeInitialize();
            }
            return _instance;
        }
    }


    protected virtual void Awake()
    {
        //custom attribute settings
        var singletonAttribute = typeof(T).GetCustomAttribute<MonoSingletonUsageAttribute>();
        singletonFlag = singletonAttribute != null ? singletonAttribute.Flag : MonoSingletonFlags.None;
        if (singletonFlag.HasFlag(MonoSingletonFlags.DontDestroyOnLoad)) DontDestroyOnLoad(gameObject);

        //error
        if (_instance is not null)
        {
            Debug.LogError("[Unity]twoSingletons_" + typeof(T).Name);
            Destroy(gameObject);
            return;
        }

        //init
        print("-AwakeInit-" + typeof(T).Name);
        if (singletonFlag.HasFlag(MonoSingletonFlags.SingletonPreset)) _instance = this as T;//GetPresetSingleton();
        else _instance = this as T;
    }
    protected virtual void OnDestroy()
    {
        if(_instance == this) _instance = null;
    }
    protected virtual void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }
    private static T GetPresetSingleton()
    {
        return null;
    }
    private static T RuntimeInitialize()
    {
        //CreateInstance
        GameObject gameObject = new(name: "Runtime_Singleton_" + typeof(T).Name);
        T result = gameObject.AddComponent<T>();
        print("Runtime_Singleton_" + typeof(T).Name);
        return result;
    }
}