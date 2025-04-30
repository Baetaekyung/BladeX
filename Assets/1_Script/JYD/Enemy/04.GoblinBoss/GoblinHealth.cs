using UnityEngine;

namespace Swift_Blade.Combat.Health
{
    public class GoblinHealth : BaseEnemyHealth
    {
        private readonly string hurtTypeHash = "HurtType";

        private int hurtType = 0;

        public override void TakeDamage(ActionData actionData)
        {
            hurtType = actionData.hurtType;
            base.TakeDamage(actionData);
        }
        
        public override void ChangeParryState()
        {
            animationController.GetAnimator().SetInteger(hurtTypeHash , hurtType);
            base.ChangeParryState();
        }
        
    }
}
