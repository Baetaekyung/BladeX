using UnityEngine;

namespace Swift_Blade.Pool
{
    [CreateAssetMenu(fileName = "PoolPrefabGameObjectSO", menuName = "SO/Pool/PrefabGameObjectSO")]
    public class PoolPrefabGameObjectSO : ScriptableObject
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
