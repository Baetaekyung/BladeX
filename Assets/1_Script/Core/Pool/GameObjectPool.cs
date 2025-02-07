using UnityEngine;

namespace Swift_Blade.Pool
{
    internal class GameObjectPool : UnityObjectPool<GameObject>
    {
        //protected override string PoolParentName => $"UnityObjectPool_{nameof(prefab)}";
        public GameObjectPool(GameObject prefab, int initialPoolCapacity = 10, int maxCapacity = 1000, int preCreate = 10) : base(prefab, initialPoolCapacity, maxCapacity, preCreate)
        {
            UnityObjectPool.SceneChangePoolDestroyEvent += base.Clear;
        }
        ~GameObjectPool()
        {
            base.Clear();
            UnityObjectPool.SceneChangePoolDestroyEvent -= base.Clear;
        }
        protected override GameObject Create()
        {
            GameObject result = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            result.SetActive(false);
            result.hideFlags = HideFlags.HideInHierarchy;
            return result;
        }
        protected override void OnDestroy(GameObject instance)
        {
            Object.Destroy(instance);
        }
        protected override void OnPop(GameObject instance)
        {
            instance.hideFlags = HideFlags.None;
            instance.SetActive(true);
        }
        protected override void OnPush(GameObject instance)
        {
            instance.hideFlags = HideFlags.HideInHierarchy;
            instance.SetActive(false);
        }
        public override void Clear()
        {
            foreach (GameObject item in poolList)
            {
                Object.Destroy(item);
            }
            base.Clear();
        }

    }
}
