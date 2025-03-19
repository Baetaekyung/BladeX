using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private readonly Dictionary<Type, IEntityComponent> componentDictionary = new();
    //private bool isAwakeInitializing;
    protected virtual void Awake()
    {
        //isAwakeInitializing = true;
        var componentList =
            GetComponentsInChildren<IEntityComponent>(true).
            ToList();
        componentList.ForEach(x => InitializeEntityComponent(x));
        //isAwakeInitializing = false;
    }
    protected virtual void Start()
    {
        var componentList = componentDictionary.Values.OfType<IEntityComponentStart>().ToList();
        componentList.ForEach(x => x.EntityComponentStart(this));
    }
    private IEntityComponent InitializeEntityComponent(IEntityComponent component)
    {
        componentDictionary.Add(component.GetType(), component);
        component.EntityComponentAwake(this);
        return component;
    }
    public T GetEntityComponent<T>() where T : Component, IEntityComponent
    {
        //Debug.Assert(!isAwakeInitializing, "don't call this in awake, call this in IEntityComponentStart.start method");

        if (componentDictionary.TryGetValue(typeof(T), out IEntityComponent value))
            return value as T;

        Debug.LogError($"[ERROR]can't find {typeof(T)}, ReInitializing...");
        T missingInstance = GetComponentInChildren<T>(true);
        if (missingInstance == null)
        {
            Debug.LogError("Can't find Component");
            return null;
        }
        
        return InitializeEntityComponent(missingInstance) as T;
    }
}
