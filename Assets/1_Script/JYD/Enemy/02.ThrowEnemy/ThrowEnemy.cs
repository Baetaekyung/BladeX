using Swift_Blade.Enemy;

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

        public override void SetDead()
        {
            base.SetDead();
            _throwEnemyAnimationController.SetStone(null);
        }                
    }
}