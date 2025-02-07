using DG.Tweening;
using Swift_Blade.Combat.Caster;
using Swift_Blade.projectile;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemAnimatorController : BaseEnemyAnimationController
    {
        public Transform target;
        
        [SerializeField] private Collider bodyCollider;

        [SerializeField] private Transform stoneTrm;
        private Projectile throwStone;
        
        private GolemBossCaster damageCaster;

        [SerializeField] private Rig rig;
        
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

        public void SetStone(Projectile _stone)
        {
            throwStone = _stone;
        }
        
        public void CatchStone()
        {
            throwStone.SetPhysicsState(true);
            throwStone.transform.SetParent(stoneTrm);
            throwStone.transform.localPosition = Vector3.zero;
        }
        
        public void ThrowStone()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            
            throwStone.SetDirection(direction);
            throwStone = null;
        }

        public void StartManualLook()
        {
            DOVirtual.Float(rig.weight, 1f, 0.3f, (value) => {
                rig.weight = value;
            });
        }
        
        public void StopManualLook()
        {
            DOVirtual.Float(rig.weight, 0f, 0.3f, (value) => {
                rig.weight = value;
            });
        }

        public override void StopAllAnimationEvents()
        {
            base.StopAllAnimationEvents();
            
            StopManualLook();
            StopManualCollider();
        }
    }
}
