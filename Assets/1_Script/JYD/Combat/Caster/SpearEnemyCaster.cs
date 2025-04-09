using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class SpearEnemyCaster : BaseEnemyCaster
    {
        
        private readonly RaycastHit[] hits = new RaycastHit[5];
        
        public override bool Cast()
        {
            if (IsNotObstacleLine() == false)
            {
                return false;
            }
            
            OnCastEvent?.Invoke();

            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + transform.forward * _castingRange;
            Vector3 direction = (endPos - startPos).normalized;
            float distance = Vector3.Distance(startPos, endPos);
            
            int hitCount = Physics.CapsuleCastNonAlloc(startPos, endPos, _casterRadius, direction, hits, distance, whatIsTarget);

            for (int i = 0; i < hitCount; i++)
            {
                RaycastHit hit = hits[i];

                if (hit.collider.gameObject == gameObject)
                    continue; // 자기 자신은 무시

                Debug.Log(hit.collider.gameObject.name);

                if (hit.collider.TryGetComponent(out IHealth health))
                {
                    Vector3 hitPoint = hit.point;
                    Vector3 hitNormal = hit.normal;

                    ActionData actionData = new ActionData(hitPoint, hitNormal, 1, true);

                    if (CanCurrentAttackParry && hit.collider.TryGetComponent(out PlayerParryController parryController))
                    {
                        bool isLookingAtAttacker = IsFacingEachOther(hit.collider.GetComponentInParent<Player>().GetPlayerTransform, transform);
                        bool canInterval = Time.time > lastParryTime + parryInterval;

                        if (parryController.GetParry() && isLookingAtAttacker && canInterval)
                        {
                            parryEvents?.Invoke();
                            parryController.ParryEvents?.Invoke();
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

                    CanCurrentAttackParry = true;
                    return true; // 하나 맞추면 바로 true 반환
                }
            }

            CanCurrentAttackParry = true;
            return false; // 아무것도 안 맞았을 경우
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
