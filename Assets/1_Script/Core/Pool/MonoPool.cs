using UnityEngine;

namespace Swift_Blade.Pool
{
    internal class MonoPool<T> : UnityObjectPool<T>
        where T : MonoBehaviour, IPoolable
    {
        //todo : remove namespace of type T
        protected override string PoolParentName => $"MonoPool_{typeof(T)}";
        public MonoPool(T prefab, int initialPoolCapacity = 100, int maxCapacity = 1000, int preCreate = 10) : base(prefab, initialPoolCapacity, maxCapacity, preCreate)
        {
            UnityObjectPool.SceneChangePoolDestroyEvent += base.Clear;
        }
        ~MonoPool()
        {
            UnityObjectPool.SceneChangePoolDestroyEvent -= base.Clear;
        }
        protected override T Create()
        {
            T result = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, poolParent);
            result.gameObject.SetActive(false);
            return result;
        }
        protected override void OnDestroy(T instance)
        {
            Object.Destroy(instance);
        }
        protected override void OnPop(T instance)
        {
            instance.Initialize();
            instance.gameObject.SetActive(true);
            instance.transform.parent = null;
        }
        protected override void OnPush(T instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.parent = poolParent;
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
