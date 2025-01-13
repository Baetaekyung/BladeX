
using System;
using UnityEngine;
using UnityEngineInternal;

namespace Swift_Blade.Boss
{
    public class SwordBoss : BossBase
    {
        [Range(1, 10)] [SerializeField] private float checkDistance;
        [SerializeField] private Transform checkTrm;

        public bool CheckBehind()
        {
            Vector3 direction = -transform.forward; 
            Ray ray = new Ray(checkTrm.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                return false;
            }

            return true; 
        }


        private void OnDrawGizmos()
        {
            if(checkTrm == null)return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(checkTrm.position ,-transform.forward *  checkDistance);
        }
        
        
        
    }
}
