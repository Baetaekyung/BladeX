using UnityEngine;

namespace Swift_Blade.Pool.CSharp
{
    internal static class ObjectGenericPool<T>
        where T : class, new()
    {
        private static readonly ObjectPool<T> objectPool;
        static ObjectGenericPool()
        {
            objectPool = new();
        }
        public static T Pop()
        {
            return objectPool.Pop();
        }
        public static void Push(T instance)
        {
            objectPool.Push(instance);
        }
        public static void Clear()
        {
            objectPool.Clear();
        }

    }
}
