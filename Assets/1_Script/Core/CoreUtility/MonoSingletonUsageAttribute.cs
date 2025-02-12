using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited =  false)]
public class MonoSingletonUsageAttribute : Attribute
{
    public MonoSingletonFlags Flag { get; }
    public MonoSingletonUsageAttribute(MonoSingletonFlags _flag)
    {
        Flag = _flag;
    }
}

[Flags]
public enum MonoSingletonFlags : ushort
{
    None            	    = 0,
    DontDestroyOnLoad       = 1 << 0,
    SingletonPreset         = 1 << 1,
}