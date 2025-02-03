using UnityEngine;

namespace Swift_Blade.Pool
{
    public static class MonoGenericPool<T>
        where T : MonoBehaviour, IPoolable
    {
        private static MonoPool<T> monoPool;
        public static ObjectPoolBase<T> GetPool => monoPool;
        /// <summary>
        /// This function must be called before calling any other methods.
        /// </summary>
        public static void Initialize(PoolPrefabMonoBehaviourSO prefabSO)
        {
            bool isMonoPoolNotInitialized = monoPool == null;
            Debug.Assert(isMonoPoolNotInitialized, $"field:monoPool is already initialized. {prefabSO.name}");

            if (isMonoPoolNotInitialized)
            {
                T prefab = prefabSO.GetMono as T;
                monoPool = new MonoPool<T>(prefab);

                Debug.Assert(prefab != null, "failed to cast Mono to T");
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
