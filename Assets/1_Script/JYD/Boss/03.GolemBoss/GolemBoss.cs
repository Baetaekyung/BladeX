using UnityEngine;

namespace Swift_Blade.Boss.Golem
{
    public class GolemBoss : BaseBoss
    {
        protected override void Update()
        {
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                attackDestination = transform.position + transform.forward;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    bossAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }
        
        
        
    }
}
