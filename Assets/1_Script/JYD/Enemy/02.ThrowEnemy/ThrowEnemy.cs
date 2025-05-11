
using Swift_Blade.Combat.Health;

namespace Swift_Blade.Enemy.Throw
{
    public class ThrowEnemy : BaseEnemy,IGetMoveSpeedAble
    {
        private ThrowAnimatorController _throwEnemyAnimationController;
        
        protected override void Start()
        {
            base.Start();
            
            _throwEnemyAnimationController = baseAnimationController as ThrowAnimatorController;
            _throwEnemyAnimationController.target = target;
        }

        public override BaseEnemyHealth GetHealth()
        {
            return baseHealth as ThrowEnemyHealth;
        }

        public override void DeadEvent()
        {
            _throwEnemyAnimationController.SetStone(null);
            base.DeadEvent();
        }
        
        public float GetMoveSpeed() => moveSpeed;
    }
}