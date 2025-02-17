using DG.Tweening;
using Swift_Blade.Combat.Caster;
using Swift_Blade.Combat.Projectile;
using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemAnimatorController : BaseEnemyAnimationController
    {
        public Transform target;

        [SerializeField] private Collider bodyCollider;

        [SerializeField] private Transform stoneTrm;
        [SerializeField] private Rig rig;

        [SerializeField] private PoolPrefabMonoBehaviourSO groundCrackSO;

        [SerializeField] private Transform rightGroundCrackTrm;
        [SerializeField] private Transform leftGroundCrackTrm;
        [SerializeField] private Transform forwardGroundCrackTrm;

        private BaseThrow _throwBaseThrow;
        private GolemBossCaster damageCaster;

        protected void Start()
        {
            damageCaster = layerCaster as GolemBossCaster;

            MonoGenericPool<GroundCrack>.Initialize(groundCrackSO);
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

        public void SetStone(BaseThrow baseThrow)
        {
            if (baseThrow == null)
                if (_throwBaseThrow != null)
                    _throwBaseThrow.transform.SetParent(null);

            _throwBaseThrow = baseThrow;
        }

        public void CatchStone()
        {
            _throwBaseThrow.SetPhysicsState(true);
            _throwBaseThrow.transform.SetParent(stoneTrm);
            _throwBaseThrow.transform.localEulerAngles = Vector3.zero;
            _throwBaseThrow.transform.localPosition = Vector3.zero;
        }

        public void ThrowStone()
        {
            var direction = (target.position - transform.position).normalized;

            _throwBaseThrow.SetDirection(direction);
            _throwBaseThrow = null;
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