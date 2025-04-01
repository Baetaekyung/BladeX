using UnityEngine;

namespace Swift_Blade.Enemy.Goblin
{
    public class GoblinEnemyInStreet : BaseGoblin
    {
        [SerializeField] private LayerMask whatIsTarget;
        [Range(1, 20)] [SerializeField] private float checkTargetRadius;
        
        private Collider[] targets;
        
        protected override void Start()
        {
            base.Start();
            btAgent.SetVariableValue("Target", (Transform)null);
            btAgent.enabled = false;

            target = null;
            
            targets = new Collider[1];
        }
        
        protected override void Update()
        {
            if (target == null)
            {
                var findTarget = FindNearTarget();
                if (findTarget != null)
                {
                    target = findTarget;
                    btAgent.BlackboardReference.SetVariableValue("Target", target);
                    btAgent.enabled = true;
                                        
                    /*var isIsLine = CanSeeTarget(findTarget);

                    if (isIsLine)
                    {
                        target = findTarget;
                        btAgent.BlackboardReference.SetVariableValue("Target", target);
                        btAgent.enabled = true;
                    }*/
                }
            }
            
            base.Update();
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            Gizmos.DrawWireSphere(transform.position, checkTargetRadius);
        }

        private Transform FindNearTarget()
        {
            targets = Physics.OverlapSphere(transform.position, checkTargetRadius, whatIsTarget);
            if (targets.Length > 0)
                return targets[0].transform;
            return null;
        }

        /*private bool CanSeeTarget(Transform target)
        {
            var directionToTarget = (target.position - transform.position).normalized;
            var distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ~whatIsTarget)) return true;
            
            return false;
        }*/
        
        
    }
}