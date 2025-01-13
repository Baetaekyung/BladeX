using Swift_Blade.Boss;
using Swift_Blade.Combat.Caster;
using UnityEngine;

namespace Swift_Blade.Boss.Golem
{
    public class GolemBossAnimatorController : BossAnimationController
    {
        [SerializeField] private Collider bodyCollider;
        
        private GolemBossCaster damageCaster;

        protected override void Start()
        {
            base.Start();
            damageCaster = layerCaster as GolemBossCaster;
        }

        public void StartManualCollider()
        {
            bodyCollider.enabled = true;
        }

        public void StopManualCollider()
        {
            bodyCollider.enabled = false;
        }

        public void JumpAttackCast()
        {
            damageCaster.JumpAttackCast();
        }
        
    }
}
