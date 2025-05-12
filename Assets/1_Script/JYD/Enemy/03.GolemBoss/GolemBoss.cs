using Swift_Blade.Enemy.Throw;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemBoss : ThrowEnemy
    {
        public override void DeadEvent()
        {
            base.DeadEvent();
            (baseAnimationController as GolemAnimatorController).StopManualLook();
        }
    }
}
