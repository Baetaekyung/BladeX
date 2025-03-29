using Swift_Blade.Combat;
using Swift_Blade.Combat.Caster;
using UnityEngine;

namespace Swift_Blade.Enemy
{
    public class SpearEnemyCaster : BaseEnemyCaster
    {
        
        public override bool Cast()
        {
            OnCastEvent?.Invoke();
    
            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + transform.forward * _castingRange;

            Collider[] hitColliders = Physics.OverlapCapsule(startPos, endPos, _casterRadius, targetLayer);
    
            bool isHit = hitColliders.Length > 0;
    
            if (isHit)
            {
                Collider hit = hitColliders[0];

                if (hit.TryGetComponent(out IDamageble health))
                {
                    Vector3 hitPoint = hit.ClosestPoint((startPos + endPos) / 2);
                    Vector3 hitNormal = (hitPoint - startPos).normalized;

                    ActionData actionData = new ActionData(hitPoint, hitNormal, 1, true);

                    if (CanCurrentAttackParry && hit.TryGetComponent(out PlayerParryController parryController))
                    {
                        /*Vector3 attacker = (hit.transform.position - transform.position).normalized;
                        Vector3 playerForward = hit.transform.forward;

                        float angle = Vector3.Angle(playerForward, attacker);
                        bool isLookingAtAttacker = angle < 70;*/
                        bool isLookingAtAttacker = true;
                        bool canInterval = Time.time > lastParryTime + parryInterval;
                        
                        if (parryController.CanParry() && isLookingAtAttacker && canInterval)
                        {
                            parryEvents?.Invoke();//적 쪽
                            parryController.ParryEvents?.Invoke();//플레이어쪽
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
