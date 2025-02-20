using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemBoss : BaseEnemy
    {
        private GolemAnimatorController golemAnimatorController;
                
        protected override void Start()
        {
            base.Start();
            
            golemAnimatorController =  (baseAnimationController as GolemAnimatorController);
            golemAnimatorController.target = target;
        }
        
        protected override void Update()
        {
            if(baseHealth.isDead)return;
                        
            if (baseAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (baseAnimationController.isManualMove && !DetectForwardObstacle())
            {
                attackDestination = transform.position + transform.forward;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    baseAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }
        
        public override void DeadEvent()
        {
            base.DeadEvent();
                        
            golemAnimatorController.SetStone(null);
        }
        
    }
}
