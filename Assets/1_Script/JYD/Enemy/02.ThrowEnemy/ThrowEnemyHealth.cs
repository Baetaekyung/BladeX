
using Swift_Blade.Enemy.Throw;

namespace Swift_Blade.Combat.Health
{
    public class ThrowEnemyHealth : BaseEnemyHealth
    {
        private bool canChangeParry;
        private ThrowAnimatorController throwAnimatorController;

        protected override void Start()
        {
            base.Start();
            throwAnimatorController = animationController as ThrowAnimatorController;
        }

        public override void ChangeParryState()
        {
            if(canChangeParry == false)return;
            
            throwAnimatorController.SetStone(null);
            base.ChangeParryState();
        }
        
        public void SetCanChangeParry(bool canChangeParry)
        {
            this.canChangeParry = canChangeParry;
        }
        
    }
}
