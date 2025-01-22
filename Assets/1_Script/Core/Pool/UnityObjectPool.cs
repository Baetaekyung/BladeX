using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade.Pool
{
    internal delegate void UnityPoolDestroyDelegate();
    internal static class UnityObjectPool
    {
        private static Transform baseParent;
        private const string BASE_PARENT_NAME = "UnityObjectPool_BaseParent";
        internal static Transform GetBaseParent => baseParent;
        internal static event UnityPoolDestroyDelegate PoolDestroyEvent;
        static UnityObjectPool()
        {
            baseParent = new GameObject(BASE_PARENT_NAME).transform;
            //Object.DontDestroyOnLoad(baseParent);
            SceneManager.sceneLoaded += (_, _) =>
            {
                if (baseParent == null)
                {
                    PoolDestroyEvent?.Invoke();
                    baseParent = new GameObject(BASE_PARENT_NAME).transform;
                }
            };
        }
    }
    internal abstract class UnityObjectPool<T> : ObjectPoolBase<T>
        where T : Object
    {
        protected readonly T prefab;
        protected readonly Transform poolParent;
        protected virtual string PoolParentName { get; set; }
        public UnityObjectPool(T prefab, int initialPoolCapacity = 10, int maxCapacity = 1000, int preCreate = 10) : base(initialPoolCapacity, maxCapacity, preCreate)
        {
            poolParent = new GameObject(PoolParentName).transform;
            poolParent.parent = UnityObjectPool.GetBaseParent;
            this.prefab = prefab;
            for (int i = 0; i < preCreate; i++)
            {
                Push(Create());
            }
        }
    }
}
