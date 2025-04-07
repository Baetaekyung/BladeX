using UnityEngine;

namespace Swift_Blade.Enemy.Goblin
{
    public class GoblinEnemyInStreet : BaseGoblin
    {
        [SerializeField] private LayerMask whatIsTarget;
        [Range(1, 20)] [SerializeField] private float checkTargetRadius;

        private Collider[] targets = new Collider[1];
        
        protected override void Start()
        {
            base.Start();
            btAgent.SetVariableValue("Target", (Transform)null);
            btAgent.enabled = false;
            
            target = null;
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
            int count = Physics.OverlapSphereNonAlloc(transform.position, checkTargetRadius, targets, whatIsTarget);
            if (count > 0)
                return targets[0].transform;
            return null;
        }
        
    }
}