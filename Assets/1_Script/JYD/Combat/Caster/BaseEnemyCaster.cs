using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Combat.Caster
{
    public class BaseEnemyCaster : LayerCaster
    {
        [SerializeField] [Range(0.5f, 10f)] protected float _casterRadius = 1f;
        [SerializeField] [Range(0f, 10f)] protected float _casterInterpolation = 0.5f;
        [SerializeField] [Range(0f, 10f)] protected float _castingRange = 1f;

        [Space(20)] public bool CanCurrentAttackParry = true;
        [Space(10)] public UnityEvent parryEvents;
        public UnityEvent unParriableAttack;
        
        protected const float parryInterval = 0.5f;
        protected float lastParryTime;
        
        public override bool Cast()
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
                ActionData actionData = new ActionData(hit.point, hit.normal, 1, true);
                
                if (CanCurrentAttackParry && hit.collider.TryGetComponent(out PlayerParryController parryController))
                {
                    bool isLookingAtAttacker = IsFacingEachOther(hit.transform.GetComponentInParent<Player>().GetPlayerTransform , transform);
                    bool canInterval = Time.time > lastParryTime + parryInterval;
                    
                    if (parryController.CanParry() && isLookingAtAttacker && canInterval)
                    {
                        parryEvents?.Invoke();//적 쪽
                        parryController.ParryEvents?.Invoke();//플레이어쪽
                        
                        lastParryTime = Time.time;
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
        
        protected bool IsFacingEachOther(Transform player, Transform enemy)
        {
            Vector3 playerToEnemy = (enemy.position - player.position).normalized;
            Vector3 enemyToPlayer = -playerToEnemy;

            float playerDot = Vector3.Dot(player.forward, playerToEnemy);
            float enemyDot = Vector3.Dot(enemy.forward, enemyToPlayer);

            return playerDot > 0.7f && enemyDot > 0.7f;
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