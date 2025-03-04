using DG.Tweening;
using Swift_Blade.Combat.Caster;
using Swift_Blade.Enemy.Throw;
using Swift_Blade.Pool;
using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemAnimatorController : ThrowAnimatorController
    {
        [SerializeField] private Collider bodyCollider;

        [SerializeField] private Rig rig;
        
        [SerializeField] private PoolPrefabMonoBehaviourSO groundCrackSO;

        [SerializeField] private Transform rightGroundCrackTrm;
        [SerializeField] private Transform leftGroundCrackTrm;
        [SerializeField] private Transform forwardGroundCrackTrm;
        
        private GolemBossCaster damageCaster;

        protected void Start()
        {
            damageCaster = layerCaster as GolemBossCaster;

            MonoGenericPool<GroundCrack>.Initialize(groundCrackSO);
        }
        
        public void JumpAttackCast()
        {
            damageCaster.JumpAttackCast();
        }
        
        public void StartManualLook()
        {
            DOVirtual.Float(rig.weight, 1f, 0.3f, value => { rig.weight = value; });
        }

        public void StopManualLook()
        {
            DOVirtual.Float(rig.weight, 0f, 0.3f, value => { rig.weight = value; });
        }

        public override void StopAllAnimationEvents()
        {
            base.StopAllAnimationEvents();

            StopManualLook();
            StopManualCollider();
        }

        public void CreateGroundCrack(int _direction)
        {
            if (_direction == 1)
            {
                var g = MonoGenericPool<GroundCrack>.Pop();
                g.transform.position = rightGroundCrackTrm.position;
            }

            if (_direction == -1)
            {
                var g = MonoGenericPool<GroundCrack>.Pop();
                g.transform.position = leftGroundCrackTrm.position;
            }

            if (_direction == 0)
            {
                var g = MonoGenericPool<GroundCrack>.Pop();
                g.transform.position = forwardGroundCrackTrm.position;
            }
            
        }
    }
}