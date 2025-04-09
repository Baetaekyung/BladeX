using UnityEngine;

namespace Swift_Blade.Enemy.Goblin
{
    public class BaseGoblin : BaseEnemy
    {
        protected GoblinAnimator goblinAnimator;
        protected Transform player;
        protected override void Start()
        {
            base.Start();
            player = target.GetComponent<Player>().GetPlayerTransform;
            goblinAnimator = baseAnimationController as GoblinAnimator;
        }

        protected override void Update()
        {
            base.Update();
            
            if (goblinAnimator.isManualKnockback && !DetectBackwardObstacle() && !baseHealth.isKnockback)
            {
                attackDestination = transform.position + player.forward;
                
                transform.position = Vector3.MoveTowards(transform.position, attackDestination,
                    goblinAnimator.knockbackSpeed * Time.deltaTime);
            }
        }
        
        private bool DetectBackwardObstacle()
        {
            var ray = new Ray(checkForward.position, -checkForward.forward);

            if (Physics.Raycast(ray, maxDistance, whatIsWall)) 
                return true;
            
            return false;
        }
        
        
    }
}