using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/Dagger")]
    public class DaggerWeaponSO : WeaponSO
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO blastParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO twingcleParticle;

        private TwingcleParticle twingcle;
        
        protected override void PlayParticle()
        {
            MonoGenericPool<MagicBlastParticle>.Initialize(blastParticle);
            MonoGenericPool<TwingcleParticle>.Initialize(twingcleParticle);
            
            MagicBlastParticle blast = MonoGenericPool<MagicBlastParticle>.Pop();
            blast.transform.SetParent(playerTransform);
            blast.transform.position = playerTransform.position + new Vector3(0,0.25f,0);
            
            twingcle = MonoGenericPool<TwingcleParticle>.Pop();
            twingcle.transform.SetParent(playerTransform);
            twingcle.transform.position = playerTransform.position + new Vector3(0,0.25f,0);
        }

        protected override void StopParticle()
        {
            MonoGenericPool<TwingcleParticle>.Push(twingcle);
        }
    }
}