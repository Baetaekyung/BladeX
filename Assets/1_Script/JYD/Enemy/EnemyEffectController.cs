using System.Collections.Generic;
using System.Collections;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using Swift_Blade.Skill;
using Unity.Behavior;
using DG.Tweening;
using UnityEngine;
using System;

namespace Swift_Blade
{
    public enum EffectType
    {
        Slow,
        Fire,
        None
    }
    
    public class EnemyEffectController : MonoBehaviour
    {
        public Dictionary<EffectType, Action<bool>> OnEffectEvents;

        [SerializeField] private PoolPrefabMonoBehaviourSO followFire;
        [SerializeField] private PoolPrefabMonoBehaviourSO iceSmoke;
                
        private Coroutine fireCoroutine;
        private const float FIRE_DAMAGE_INTERVAL = 0.3f;
        private readonly WaitForSeconds fireWait = new WaitForSeconds(FIRE_DAMAGE_INTERVAL);
                
        private BaseEnemy enemy;
        private BaseEnemyAnimationController baseEnemyAnimationController;
        private BehaviorGraphAgent btAgent;
        
        private void Awake()
        {
            OnEffectEvents = new Dictionary<EffectType, Action<bool>>();
            enemy = GetComponent<BaseEnemy>();
            btAgent = GetComponent<BehaviorGraphAgent>();
            baseEnemyAnimationController = GetComponentInChildren<BaseEnemyAnimationController>();
            
            MonoGenericPool<FollowFireParticle>.Initialize(followFire);
            MonoGenericPool<IceSmokeParticle>.Initialize(iceSmoke);
            
        }
        
        
        public void SetSlow(float speed, float duration = 0)
        {
            if (OnEffectEvents.TryGetValue(EffectType.Slow, out var action))
                action?.Invoke(true);

            PlayIceSmokeParticle();
            
            baseEnemyAnimationController.SetAnimationSpeed(speed);
            baseEnemyAnimationController.MultiplyDefaultAttackMoveSpeed(0.5f);
            btAgent.SetVariableValue("MoveSpeed",enemy.GetMoveSpeed() * 0.5f);
            
            if(duration > 0)
                DOVirtual.DelayedCall(duration, ResetSlow);
        }
        
        public void SetFire(float damage,float duration)
        {
            if (OnEffectEvents.TryGetValue(EffectType.Fire, out var action))
                action?.Invoke(true);
            
            if (fireCoroutine != null)
            {
                ResetFire();
            }
            fireCoroutine = StartCoroutine(FireDamageRoutine(damage));
                        
            PlayFireParticle(duration);
            
            if (duration > 0)
            {
                DOVirtual.DelayedCall(duration, ResetFire);
            }
        }
        private void ResetSlow()
        {
            OnEffectEvents[EffectType.Slow]?.Invoke(false);
            
            baseEnemyAnimationController.ResetAnimationSpeed();
            baseEnemyAnimationController.ResetDefaultMoveSpeed();
            btAgent.SetVariableValue("MoveSpeed", enemy.GetMoveSpeed());
        }
        
        private void ResetFire()
        {
            OnEffectEvents[EffectType.Fire]?.Invoke(false);
            
            
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
        }
        
        private void PlayFireParticle(float duration)
        {
            FollowFireParticle fireParticle = MonoGenericPool<FollowFireParticle>.Pop();
            fireParticle.SetDuration(duration);
            
            fireParticle.transform.position = transform.position + new Vector3(0,0.67f,0);
            fireParticle.SetFollowTransform(transform);
        }
                
        private void PlayIceSmokeParticle()
        { 
            MonoGenericPool<IceSmokeParticle>.Pop().transform.position = transform.position;
        }
        
        private IEnumerator FireDamageRoutine(float damage)
        {
            while (true)
            {
                ActionData actionData = new ActionData
                {
                    damageAmount = damage,
                    textColor = Color.yellow,
                    hitPoint = transform.position
                };
                
                enemy.GetHealth().TakeDamage(actionData);
                
                yield return fireWait;
            }
        }
        
        
        
    }
}
