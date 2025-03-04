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
            MonoGenericPool<DustUp>.Initialize(dustUpEffect);    
            MonoGenericPool<GroundCrack>.Initialize(groundEffect);    
            MonoGenericPool<Prick>.Initialize(prickEffect);    
        }
        
        public void PlayDustUpEffect()
        {
            if (Physics.Raycast(dustUpTransform.position ,Vector3.down ,out RaycastHit hit, 1f , groundLayer))
            {
                DustUp dust = MonoGenericPool<DustUp>.Pop();
                dust.transform.position = hit.point;
                
                GroundCrack groundCrack = MonoGenericPool<GroundCrack>.Pop();
                groundCrack.transform.position = hit.point;
                
            }
        }

        public void PlayPrickEffect()
        {
            Prick prick = MonoGenericPool<Prick>.Pop();
            prick.transform.rotation = prickTrm1.rotation;
            prick.transform.position = prickTrm1.position;
        }
        
        public void PlayPrickEffect1()
        {
            Prick prick = MonoGenericPool<Prick>.Pop();
            prick.transform.rotation = prickTrm2.rotation;
            prick.transform.position = prickTrm2.position;
        }
        
    }
}
