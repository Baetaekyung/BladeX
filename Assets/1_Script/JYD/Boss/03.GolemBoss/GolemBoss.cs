using UnityEngine;

namespace Swift_Blade.Boss.Golem
{
    public class GolemBoss : BossBase
    {
        protected override void Update()
        {
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                attackDestination = transform.position + transform.forward * 1f;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    bossAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }
        
        
        
    }
}
