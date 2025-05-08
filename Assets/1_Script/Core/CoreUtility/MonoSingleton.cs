using UnityEngine;
using System.Reflection;
using System;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static MonoSingletonFlags? singletonFlag;
    private static T instance = null;
    public static event Action OnSingletonDestroy;

    public static T Instance
    {
        get
        {
            if (instance is null)//UnityEngine.Object's implicit bool operator detour.
            {
                //tood : if scene transitinoing, throw error
                if (IsShuttingDown) return null;

                instance = RuntimeInitialize();
            }
            return instance;
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
        if (instance != null)
        {
            Destroy(gameObject);
            throw new Exception("[ERROR]TwoSingletons_" + typeof(T).Name);
            //Debug.LogError("[ERROR]TwoSingletons_" + typeof(T).Name);
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
        instance = this as T;

    }
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            OnSingletonDestroy?.Invoke();
            instance = null;
        }

    }
    protected virtual void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }
}