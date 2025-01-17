using UnityEngine;

namespace Swift_Blade.Boss.Goblin
{
    public class GoblinAnimatorController : BossAnimationController
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
        
        public void CreateSummon()
        {
            (boss as GoblinBoss).Summon();
        }

        public void SetAnimationSpeed(float _speed)
        {
            //Animator.SetFloat("AnimationEnd",_speed);
        }
        
                
    }
}
