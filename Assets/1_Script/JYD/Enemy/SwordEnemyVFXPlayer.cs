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

        public void PlayPrickEffect()
        {
            PrickParticle prickParticle = MonoGenericPool<PrickParticle>.Pop();
            prickParticle.transform.rotation = prickTrm1.rotation;
            prickParticle.transform.position = prickTrm1.position;
        }
        
        public void PlayPrickEffect1()
        {
            PrickParticle prickParticle = MonoGenericPool<PrickParticle>.Pop();
            prickParticle.transform.rotation = prickTrm2.rotation;
            prickParticle.transform.position = prickTrm2.position;
        }
        
    }
}
