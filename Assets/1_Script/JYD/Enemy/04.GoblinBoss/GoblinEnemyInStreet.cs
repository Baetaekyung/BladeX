using System;
using Swift_Blade.Enemy.Goblin;
using Unity.Behavior;
using UnityEngine;

namespace Swift_Blade.Enemy.Golbin
{
    public class GoblinEnemyInStreet : BaseGoblin
    {
        private Collider[] targets;
        
        [SerializeField] private LayerMask whatIsTarget;
        [Range(1, 10)] [SerializeField] private float checkTargetRadius;
        
        protected override void Start()
        {
            base.Start();
            
            btAgent.enabled = false;
            targets = new Collider[10];
        }

        protected override void Update()
        {
            Transform findTarget = FindNearTarget();
            if (findTarget != null)
            {
                bool isIsLine = CanSeeTarget(findTarget);
                
                if (isIsLine)
                {
                    target = findTarget;
                    btAgent.BlackboardReference.SetVariableValue("Target", target);
                    btAgent.enabled = true;
                }
            }
            
            base.Update();
        }

        private Transform FindNearTarget()
        {
            targets = Physics.OverlapSphere(transform.position, checkTargetRadius, whatIsTarget);
            if (targets.Length > 0)
                return targets[0].transform;
            return null;
        }

        private bool CanSeeTarget(Transform target)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ~whatIsTarget))
            {
                return true; 
            }

            return false; 
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position , checkTargetRadius);
        }
    }
}
