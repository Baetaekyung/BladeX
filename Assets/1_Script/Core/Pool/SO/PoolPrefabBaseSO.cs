using UnityEngine;

namespace Swift_Blade.Pool
{
    public abstract class PoolPrefabBaseSO : ScriptableObject
    {
        [SerializeField] private int preCreateAmount;
        [SerializeField] protected GameObject prefab;
        private int hash;
        public int GetPreCreate => preCreateAmount;
        public GameObject GetPrefab => prefab;
        public int GetHash => hash;
        protected virtual void OnValidate()
        {
            hash = prefab.GetHashCode();
        }
    }
}
