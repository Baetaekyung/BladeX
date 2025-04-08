using System;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Enemy.Sword
{
    public class SwordEnemyVFXPlayer : MonoBehaviour
    {
        [Header("DustUp Info")]
        [SerializeField] private PoolPrefabMonoBehaviourSO dustUpEffect;
        [SerializeField] private Transform dustUpTransform;
        [SerializeField] private LayerMask groundLayer;
        
        [Header("GroundCrack Info")]
        [SerializeField] private PoolPrefabMonoBehaviourSO groundEffect;
        
        [Header("Prick Info")]
        [SerializeField] private PoolPrefabMonoBehaviourSO prickEffect;
        [SerializeField] private Transform prickTrm1;
        [SerializeField] private Transform prickTrm2;
        
        private void Start()
        {
            MonoGenericPool<DustUpParticle>.Initialize(dustUpEffect);    
            MonoGenericPool<GroundCrackParticle>.Initialize(groundEffect);    
            MonoGenericPool<PrickParticle>.Initialize(prickEffect);    
        }
        
        public void PlayDustUpEffect()
        {
            if (Physics.Raycast(dustUpTransform.position ,Vector3.down ,out RaycastHit hit, 1f , groundLayer))
            {
                DustUpParticle dust = MonoGenericPool<DustUpParticle>.Pop();
                dust.transform.position = hit.point;
                
                GroundCrackParticle groundCrackParticle = MonoGenericPool<GroundCrackParticle>.Pop();
                groundCrackParticle.transform.position = hit.point;
                
            }
        }

        private void PlayPrickEffectAt(Transform targetTrm)
        {
            PrickParticle prickParticle = MonoGenericPool<PrickParticle>.Pop();
            prickParticle.transform.SetPositionAndRotation(targetTrm.position , targetTrm.rotation);
        }
        
        public void PlayPrickEffect()
        {
            PlayPrickEffectAt(prickTrm1);
        }

        public void PlayPrickEffect1()
        {
            PlayPrickEffectAt(prickTrm2);
        }
        
    }
}
