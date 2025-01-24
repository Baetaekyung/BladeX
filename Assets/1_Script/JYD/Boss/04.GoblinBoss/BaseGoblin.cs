using System;
using Unity.Behavior;
using UnityEngine;

namespace Swift_Blade.Boss.Goblin
{
    public class BaseGoblin : BaseBoss
    {
        [Range(0,2)][SerializeField] protected float stopDistance;
        
      
        protected GoblinAnimator goblinAnimator;
        
        protected override void Start()
        {
            base.Start();
            
            
            goblinAnimator = bossAnimationController as GoblinAnimator;
        }

        protected override void Update()
        {
            if(baseHealth.isDead)return;
            
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

            if (goblinAnimator.isManualKnockback)
            {
                attackDestination = transform.position + -transform.forward;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    goblinAnimator.knockbackSpeed * Time.deltaTime); 
            }
            
        }
                
    }
}
