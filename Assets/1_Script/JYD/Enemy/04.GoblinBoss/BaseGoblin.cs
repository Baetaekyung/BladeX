using UnityEngine;

namespace Swift_Blade.Enemy.Goblin
{
    public class BaseGoblin : BaseEnemy
    {
        protected GoblinAnimator goblinAnimator;

        protected override void Start()
        {
            base.Start();
            
            goblinAnimator = baseAnimationController as GoblinAnimator;
        }

        protected override void Update()
        {
            if (baseHealth.isDead)
                return;

            if (baseAnimationController.isManualRotate) 
                FactToTarget(target.position);

            if (baseAnimationController.isManualMove && !DetectForwardObstacle())
            {
                var distance = Vector3.Distance(transform.position, target.position);

                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                        baseAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }

            if (goblinAnimator.isManualKnockback && !DetectBackwardObstacle())
            {
                attackDestination = transform.position + -transform.forward;
                
                transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                    goblinAnimator.knockbackSpeed * Time.deltaTime);
            }
        }
        
        protected bool DetectBackwardObstacle()
        {
            var ray = new Ray(checkForward.position, -checkForward.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, whatIsWall)) return true;
            return false;
        }
        
        
    }
}