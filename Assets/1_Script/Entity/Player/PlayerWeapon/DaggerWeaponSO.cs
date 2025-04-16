using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/Dagger")]
    public class DaggerWeaponSO : WeaponSO
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO blastParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO twingcleParticle;
        
        private List<TwingcleParticle> twingcles = new List<TwingcleParticle>(10);
        
        protected override void PlayParticle()
        {
            MonoGenericPool<MagicBlastParticle>.Initialize(blastParticle);
            MonoGenericPool<TwingcleParticle>.Initialize(twingcleParticle);
            
            MagicBlastParticle blast = MonoGenericPool<MagicBlastParticle>.Pop();
            blast.transform.SetParent(playerTransform);
            blast.transform.position = playerTransform.position + new Vector3(0,0.25f,0);
            
            TwingcleParticle twingcle = MonoGenericPool<TwingcleParticle>.Pop();
            twingcle.transform.SetParent(playerTransform);
            twingcle.transform.position = playerTransform.position + new Vector3(0,0.25f,0);
            
            twingcles.Add(twingcle);
        }

        protected override void StopParticle()
        {
            foreach (var item in twingcles)
            {
                MonoGenericPool<TwingcleParticle>.Push(item);
            }
            twingcles.Clear();
        }
    }
}