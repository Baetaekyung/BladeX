using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade.Pool
{
    internal class MonoPool<T> : UnityObjectPool<T>
        where T : MonoBehaviour, IPoolable
    {
        public MonoPool(T prefab, int initialPoolCapacity = 100, int maxCapacity = 1000, int preCreate = 10) 
            : base(prefab, initialPoolCapacity, maxCapacity, preCreate)
        {
            UnityObjectPool.SceneChangePoolDestroyEvent += base.Clear;
        }
        ~MonoPool()
        {
            base.Clear();
            UnityObjectPool.SceneChangePoolDestroyEvent -= base.Clear;
        }
        public override T Pop()
        {
            T instance = base.Pop();
            instance.gameObject.hideFlags = HideFlags.None;
            instance.gameObject.SetActive(true);
            instance.OnPop();
            //SceneManager.MoveGameObjectToScene(instance.gameObject, SceneManager.GetActiveScene());
            return instance;
        }
        public override void Push(T instance)
        {
            instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
            instance.gameObject.SetActive(false);
            instance.OnPush();

            //experimental.
            //bool hasParent = instance.transform.parent != null;
            //if (hasParent)
            //{
            //    instance.transform.SetParent(null);
            //    //SceneManager.MoveGameObjectToScene(instance.gameObject, SceneManager.GetActiveScene());
            //}

            //Object.DontDestroyOnLoad(instance);

            base.Push(instance);
        }
        protected override T Create()
        {
            T result = base.Create();
            result.gameObject.SetActive(false);
            result.OnCreate();
            //Object.DontDestroyOnLoad(result);
            return result;
        }
        public override void Clear()
        {
            foreach (T item in poolList)
            {
                Object.Destroy(item.gameObject);
            }   
            base.Clear();
        }
    }
}
