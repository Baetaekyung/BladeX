
namespace Swift_Blade.Enemy.Throw
{
    public class ThrowEnemy :  BaseEnemy,IGetSpeedAble
    {
        private ThrowAnimatorController _throwEnemyAnimationController;
        
        protected override void Start()
        {
            base.Start();
            
            _throwEnemyAnimationController = baseAnimationController as ThrowAnimatorController;
            _throwEnemyAnimationController.target = target;
        }
        
        public float GetSpeed() => moveSpeed;
                
        public override void DeadEvent()
        {
            base.DeadEvent();
            _throwEnemyAnimationController.SetStone(null);
        }
        
    }
}