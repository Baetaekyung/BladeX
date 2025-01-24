using System;
using UnityEngine;

namespace Swift_Blade.Pool.CSharp
{
    internal struct PooledObject<T> : IDisposable
        where T : class, new()
    {
        public T Value => value;
        private readonly T value;
        public PooledObject(T instance = null)
        {
            if (instance == null)
                value = new();
            else
                value = instance;
        }
        public void Dispose()
        {
            ObjectGenericPool<T>.Push(value);
        }
    }
}
