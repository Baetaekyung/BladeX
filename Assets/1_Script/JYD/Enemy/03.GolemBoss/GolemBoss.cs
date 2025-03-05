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
        
        
        public override void DeadEvent()
        {
            base.DeadEvent();
                        
            golemAnimatorController.SetStone(null);
        }
        
    }
}
