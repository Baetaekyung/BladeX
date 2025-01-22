using UnityEngine;

namespace Swift_Blade.Pool
{
    internal class GameObjectPool : UnityObjectPool<GameObject>
    {
        protected override string PoolParentName => $"UnityObjectPool_{nameof(prefab)}";
        public GameObjectPool(GameObject prefab, int initialPoolCapacity = 10, int maxCapacity = 1000, int preCreate = 10) : base(prefab, initialPoolCapacity, maxCapacity, preCreate)
        {
            UnityObjectPool.PoolDestroyEvent += base.Clear;
        }
        ~GameObjectPool()
        {
            UnityObjectPool.PoolDestroyEvent -= base.Clear;
        }
        protected override GameObject Create()
        {
            GameObject result = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, poolParent);
            result.SetActive(false);
            return result;
        }
        protected override void OnDestroy(GameObject instance)
        {
            Object.Destroy(instance);
        }
        protected override void OnPop(GameObject instance)
        {
            instance.SetActive(true);
        }
        protected override void OnPush(GameObject instance)
        {
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
