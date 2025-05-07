using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemBoss : BaseEnemy,IGetSpeedAble
    {
        private GolemAnimatorController golemAnimatorController;
        
        protected override void Start()
        {
            base.Start();
            
            golemAnimatorController =  (baseAnimationController as GolemAnimatorController);
            golemAnimatorController.target = target;
        }

        public float GetSpeed()
        {
            return moveSpeed;
        }
        
    }
}
