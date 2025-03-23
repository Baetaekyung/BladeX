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
            base.Update();
            
            if (goblinAnimator.isManualKnockback && !DetectBackwardObstacle() && !baseHealth.isKnockback)
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