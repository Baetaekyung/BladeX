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
        public UnityEvent unParriableAttack;
        
        public override bool CastDamage()
        {
            OnCastEvent?.Invoke();
            
            Vector3 startPos = GetStartPosition();

            bool isHit = Physics.SphereCast(
                startPos,
                _casterRadius,
                transform.forward,
                out RaycastHit hit,
                _castingRange, targetLayer);
            
            if (isHit && hit.collider.TryGetComponent(out IDamageble health))
            {
                ActionData actionData = new ActionData(hit.point, hit.normal, 1,transform , true);
                
                if (CanCurrentAttackParry && hit.collider.TryGetComponent(out PlayerParryController parryController))
                {
                    /*Vector3 attacker = (hit.transform.position - transform.position).normalized;
                    Vector3 playerForward = hit.transform.forward;
                    
                    float angle = Vector3.Angle(playerForward, attacker);
                    bool isLookingAtAttacker = angle < 70;*/
                    bool isLookingAtAttacker = true;
                    
                    if (parryController.CanParry() && isLookingAtAttacker)
                    {
                        parryEvents?.Invoke();//적 쪽
                        parryController.ParryEvents?.Invoke();//플레이어쪽
                    }
                    else
                    {
                        ApplyDamage(health,actionData);
                    }
                }
                else
                {
                    ApplyDamage(health,actionData);
                }
            }

            CanCurrentAttackParry = true;

            return isHit;
        }

        protected virtual void ApplyDamage(IDamageble health,ActionData actionData)
        {
            OnCastDamageEvent?.Invoke(actionData);
            health.TakeDamage(actionData);
        }

        public Vector3 GetStartPosition()
        {
            return transform.position + transform.forward * -_casterInterpolation * 2;
        }

        public void DisableParryForCurrentAttack()
        {
            unParriableAttack?.Invoke();
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