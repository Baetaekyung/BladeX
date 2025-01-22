using System;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public static class MonoGenericPool<T>
        where T : MonoBehaviour, IPoolable
    {
        private static MonoPool<T> monoPool;
        //private static MonoPool<T> MonoPool
        //{
        //    get
        //    {
        //        Debug.Assert(monoPool != null, "field:monoPool is not initialized.");
        //        return monoPool;
        //    }
        //}
        /// <summary>
        /// This function must be called before calling any other methods.
        /// </summary>
        public static void Initialize(PoolPrefabMonoBehaviourSO prefabSO)
        {
            bool collisionCheck = monoPool == null;
            Debug.Assert(collisionCheck, $"field:monoPool is already initialized. {prefabSO.name}");

            if (collisionCheck)
            {
                T prefab = prefabSO.GetMono as T;
                monoPool = new MonoPool<T>(prefab);

                Debug.Assert(prefab != null, "failed to cast Mono to T");//Debug.Log(prefabSO.GetMono as T == null);
            }
        }
        public static T Pop()
        {
            Debug.Assert(monoPool != null, "field:monoPool is not initialized.");
            return monoPool.Pop();
        }
        public static void Push(T instance)
        {
            Debug.Assert(monoPool != null, "field:monoPool is not initialized.");
            monoPool.Push(instance);
        }
        public static void Clear()
        {
            Debug.Assert(monoPool != null, "field:monoPool is not initialized.");
            monoPool.Clear();
        }

        public static int Dbg_print()
        {
            return monoPool.Dbg_Cnt();
        }
    }
}
