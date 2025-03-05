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

        public override void DeadEvent()
        {
            base.DeadEvent();
            _throwEnemyAnimationController.SetStone(null);
        }       
        
    }
}