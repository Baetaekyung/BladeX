using Swift_Blade.Combat.Caster;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat.Caster
{
    public class BaseBossCaster : LayerCaster
    {
        [SerializeField] [Range(0.5f, 10f)] protected float _casterRadius = 1f;
        [SerializeField] [Range(0f, 10f)] protected float _casterInterpolation = 0.5f;
        [SerializeField] [Range(0f, 10f)] protected float _castingRange = 1f;

        [Space(20)] public bool CanCurrentAttackParry = true;
        [Space(10)] public UnityEvent parryEvents;

        public override bool CastDamage()
        {
            Vector3 startPos = GetStartPosition();

            bool isHit = Physics.SphereCast(
                startPos,
                _casterRadius,
                transform.forward,
                out RaycastHit hit,
                _castingRange, targetLayer);

            if (isHit && hit.collider.TryGetComponent(out IDamageble health))
            {
                if (CanCurrentAttackParry && hit.collider.TryGetComponent(out PlayerParryController parryController))
                {
                    if (parryController.CanParry())
                    {
                        parryEvents?.Invoke();
                        print("패링성공함!!!");
                    }
                    else
                    {
                        ApplyDamage(health);
                    }
                }
                else
                {
                    ApplyDamage(health);
                }
            }

            CanCurrentAttackParry = true;

            return isHit;
        }

        protected virtual void ApplyDamage(IDamageble health)
        {
            OnCastDamageEvent?.Invoke();

            ActionData actionData = new ActionData
            {
                damageAmount = 10,
                knockbackDir = transform.forward,
                knockbackDuration = 0.2f,
                knockbackPower = 5,
                dealer = transform
            };

            health.TakeDamage(actionData);
        }

        public Vector3 GetStartPosition()
        {
            return transform.position + transform.forward * -_casterInterpolation * 2;
        }

        public void DisableParryForCurrentAttack()
        {
            CanCurrentAttackParry = false;
        }
        
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetStartPosition(), _casterRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetStartPosition() + transform.forward * _castingRange, _casterRadius);
            Gizmos.color = Color.white;

        }
    }
}