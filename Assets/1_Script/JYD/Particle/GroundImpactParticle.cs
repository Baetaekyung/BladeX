
using UnityEngine;

namespace Swift_Blade.Pool
{
    public class GroundImpactParticle : ParticlePoolAble<GroundImpactParticle> 
    {
        [SerializeField] private PoolPrefabGameObjectSO groundImpactPrefab;
        protected override void Push()
        {
            GameObjectPoolManager.Push(groundImpactPrefab , gameObject);
        }
        
    }
}
