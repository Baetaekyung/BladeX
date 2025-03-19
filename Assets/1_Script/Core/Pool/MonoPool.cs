using UnityEngine;

namespace Swift_Blade.Pool
{
    internal class MonoPool<T> : UnityObjectPool<T>
        where T : MonoBehaviour, IPoolable
    {
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
            result.OnCreate();
            result.gameObject.hideFlags = HideFlags.HideInHierarchy;
            result.gameObject.SetActive(false);
            return result;
        }
        protected override void Destroy(T instance)
        {
            Object.Destroy(instance);
        }
        public override T Pop()
        {
            T instance = base.Pop();
            instance.OnPop();
            instance.gameObject.hideFlags = HideFlags.None;
            instance.gameObject.SetActive(true);
            return instance;
        }
        public override void Push(T instance)
        {
            instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
            instance.OnPush();
            instance.gameObject.SetActive(false);
            base.Push(instance);
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
