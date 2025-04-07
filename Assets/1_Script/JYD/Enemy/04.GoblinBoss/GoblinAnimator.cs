using UnityEngine;

namespace Swift_Blade.Enemy.Goblin
{
    public class GoblinAnimator : BaseEnemyAnimationController
    {
        [Range(1, 100)] public float knockbackSpeed;

        public bool isManualKnockback;

        public void StartManualKnockback()
        {
            isManualKnockback = true;
            NavMeshAgent.enabled = false;
        }

        public void StopManualKnockback()
        {
            isManualKnockback = false;
            NavMeshAgent.enabled = true;
        }

        public void SetAnimationSpeed(float _speed)
        {
            if (Animator == null) 
                Animator = GetComponent<Animator>();
            
            SetAttackAnimationSpeed();
        }

        public override void StopAllAnimationEvents()
        {
            base.StopAllAnimationEvents();
            StopManualKnockback();
        }
    }
}