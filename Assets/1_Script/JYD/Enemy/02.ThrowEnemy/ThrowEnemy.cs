using UnityEngine;

namespace Swift_Blade.Enemy.Throw
{
    public class ThrowEnemy :  BaseEnemy
    {
        private ThrowAnimatorController _throwEnemyAnimationController;
        
        protected override void Start()
        {
            base.Start();
            
            _throwEnemyAnimationController = baseAnimationController as ThrowAnimatorController;
            _throwEnemyAnimationController.target = target;
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
            _throwEnemyAnimationController.SetStone(null);
        }       
        
    }
}