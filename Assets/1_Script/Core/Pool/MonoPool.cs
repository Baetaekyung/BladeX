using UnityEngine;

namespace Swift_Blade.Pool
{
    internal class MonoPool<T> : UnityObjectPool<T>
        where T : MonoBehaviour, IPoolable
    {
        //protected override string PoolParentName => $"MonoPool_{typeof(T)}";
        public MonoPool(T prefab, int initialPoolCapacity = 100, int maxCapacity = 1000, int preCreate = 10) : base(prefab, initialPoolCapacity, maxCapacity, preCreate)
        {
            UnityObjectPool.SceneChangePoolDestroyEvent += base.Clear;
        }
        ~MonoPool()
        {
            base.Clear();
            UnityObjectPool.SceneChangePoolDestroyEvent -= base.Clear;
        }
        protected override T Create()
        {
            T result = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            result.gameObject.SetActive(false);
            result.hideFlags = HideFlags.HideInHierarchy;
            return result;
        }
        protected override void OnDestroy(T instance)
        {
            Object.Destroy(instance);
        }
        protected override void OnPop(T instance)
        {
            instance.OnPopInitialize();
            instance.hideFlags = HideFlags.None;
            instance.gameObject.SetActive(true);
        }
        protected override void OnPush(T instance)
        {
            instance.hideFlags = HideFlags.HideInHierarchy;
            instance.gameObject.SetActive(false);
        }

        public override void Clear()
        {
            foreach (T item in poolList)
            {
                Object.Destroy(item);
            }
            base.Clear();
        }
    }
}
