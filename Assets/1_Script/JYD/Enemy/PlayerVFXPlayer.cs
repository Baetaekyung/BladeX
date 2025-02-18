using Swift_Blade.Pool;
using Swift_Blade.Pool.Dust;
using Swift_Blade.Pool.HitSlash;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVFXPlayer : MonoBehaviour
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO dustEffect;
        [SerializeField] private PoolPrefabMonoBehaviourSO hitSlashEffect;
        
        private void Start()
        {
            MonoGenericPool<Dust>.Initialize(dustEffect);
            MonoGenericPool<HitSlash>.Initialize(hitSlashEffect);
        }
        
        public void PlayHitEffect(ActionData actionData)
        {
            Dust dust = MonoGenericPool<Dust>.Pop();
            dust.transform.position = actionData.hitPoint;
            
            HitSlash hitSlash = MonoGenericPool<HitSlash>.Pop();
            hitSlash.transform.position = actionData.hitPoint;
        }
        
    }
}
