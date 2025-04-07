using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class SpearEnemyCaster : BaseEnemyCaster
    {
        
        public override bool Cast()
        {
            OnCastEvent?.Invoke();
            
            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + transform.forward * _castingRange;
            Vector3 direction = (endPos - startPos).normalized;
            float distance = Vector3.Distance(startPos, endPos);
            
            bool isHit = Physics.CapsuleCast(startPos, endPos, _casterRadius, direction, out RaycastHit hit, distance, targetLayer);
            
            if (isHit)
            {
                if (hit.collider.TryGetComponent(out IHealth health))
                {
                    Vector3 hitPoint = hit.point;
                    Vector3 hitNormal = hit.normal;

                    ActionData actionData = new ActionData(hitPoint, hitNormal, 1, true);

                    if (CanCurrentAttackParry && hit.collider.TryGetComponent(out PlayerParryController parryController))
                    {
                        bool isLookingAtAttacker = IsFacingEachOther(hit.collider.GetComponentInParent<Player>().GetPlayerTransform, transform);
                        bool canInterval = Time.time > lastParryTime + parryInterval;

                        if (parryController.CanParry() && isLookingAtAttacker && canInterval)
                        {
                            parryEvents?.Invoke(); // 적 쪽
                            parryController.ParryEvents?.Invoke(); // 플레이어 쪽
                        }
                        else
                        {
                            ApplyDamage(health, actionData);
                        }
                    }
                    else
                    {
                        ApplyDamage(health, actionData);
                    }
                }
            }

            CanCurrentAttackParry = true;

            return isHit;
        }

        protected override void OnDrawGizmosSelected()
        {
            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + transform.forward * _castingRange;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPos, _casterRadius);
            Gizmos.DrawWireSphere(endPos, _casterRadius);
            Gizmos.DrawLine(startPos, endPos);
        }
        
    }
    
}
