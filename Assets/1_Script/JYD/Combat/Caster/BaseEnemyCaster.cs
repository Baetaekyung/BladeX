using UnityEngine.Events;
using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class BaseEnemyCaster : LayerCaster
    {
        [Space(20)] public bool CanCurrentAttackParry = true;
        [Space(10)] public UnityEvent parryEvents;
        public UnityEvent unParriableAttack;
        
        protected const float parryInterval = 0.5f;
        protected float lastParryTime;
        
        public override bool Cast()
        {
            if (IsNotObstacleLine() == false)
            {
                return false;
            }
            
            OnCastEvent?.Invoke();
            
            Vector3 startPos = GetStartPosition();
            
            bool isHit = Physics.SphereCast(
                startPos,
                _casterRadius,
                transform.forward,
                out RaycastHit hit,
                _castingRange, whatIsTarget);
            
            if (isHit && hit.collider.TryGetComponent(out IHealth health))
            {
                ActionData actionData = new ActionData(hit.point, hit.normal, 1, true);
                
                if (CanCurrentAttackParry && hit.collider.TryGetComponent(out PlayerParryController parryController))
                {
                    TryParry(hit, parryController, health, actionData);
                }
                else
                {
                    ApplyDamage(health,actionData);
                }
            }

            CanCurrentAttackParry = true;

            return isHit;
        }

        private void TryParry(RaycastHit hit, PlayerParryController parryController, IHealth health, ActionData actionData)
        {
            bool isLookingAtAttacker = IsFacingEachOther(hit.transform.GetComponentInParent<Player>().GetPlayerTransform , transform);
            bool canInterval = Time.time > lastParryTime + parryInterval;
            
            if (parryController.GetParry() && isLookingAtAttacker && canInterval)
            {
                parryEvents?.Invoke();//적 쪽
                parryController.ParryEvents?.Invoke();
                
                lastParryTime = Time.time;
            }
            else
            {
                ApplyDamage(health,actionData);
            }
        }
                        
        public void DisableParryForCurrentAttack()
        {
            unParriableAttack?.Invoke();
            CanCurrentAttackParry = false;
        }
        
        protected bool IsFacingEachOther(Transform player, Transform enemy)
        {
            Vector3 playerToEnemy = (enemy.position - player.position).normalized;
           //Debug.DrawRay(enemy.position + new Vector3(0,1,0) , enemy.forward, Color.red , 5);
           //Debug.DrawRay(enemy.position + new Vector3(0,1,0) , playerToEnemy, Color.red , 5);
            
            float playerDot = Vector3.Dot(player.forward, playerToEnemy);
            
            return playerDot > 0;
        }
        
        

    }
}