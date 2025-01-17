using UnityEngine;

namespace Swift_Blade.Boss.Goblin
{
    public class GoblinBaseBoss : BaseBoss
    {
        public float stopDistance;
        
        protected override void Update()
        {
    
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                float distance = Vector3.Distance(transform.position , target.position);

                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                        bossAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
                
            }
        }
    }
    
    
    
    
}
