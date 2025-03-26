using UnityEngine;

namespace Swift_Blade.Pool
{
    [CreateAssetMenu(fileName = "PoolPrefabMonoBehaviourSO", menuName = "SO/Pool/PrefabMonoBehaviourSO")]
    public class PoolPrefabMonoBehaviourSO : PoolPrefabGameObjectSO
    {
        [SerializeField] private MonoBehaviour mono;
        public MonoBehaviour GetMono => mono;
        protected override void OnValidate()
        {
            base.OnValidate();
            if (mono != null)
            {
                if (mono.gameObject != prefab)
                {
                    mono = null;
                    Debug.LogError($"Mono and prefab GameObjects are different. {name}");
                }
            }
        }
    }
}
