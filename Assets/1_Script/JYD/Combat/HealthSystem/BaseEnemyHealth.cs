using System.Collections;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using UnityEngine.AI;
using Unity.Behavior;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Swift_Blade.Combat.Health
{
    public class BaseEnemyHealth : BaseEntityHealth
    {
        public event Action<float> OnChangeHealthEvent; 
        public float currentHealth;
        
        [Space]
        [SerializeField] protected BehaviorGraphAgent BehaviorGraphAgent;
        [SerializeField] protected ChangeBossState changeBossState;
        
        protected BaseEnemyAnimationController animationController;
        private Rigidbody enemyRigidbody;
        private NavMeshAgent navMeshAgent;
        
        [Header("Knockback info")]
        public bool isKnockback = false;

        [Header("EXP")] 
        [SerializeField] private int minExp;
        [SerializeField] private int maxExp;
        
        
        private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        
        private const float DAMAGE_INTERVAL = 0.1f;
        protected float lastDamageTime;
        
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            BehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            animationController = GetComponentInChildren<BaseEnemyAnimationController>();
            
            OnHitEvent.AddListener(StartKnockback);
            OnHitEvent.AddListener(GeneratorText);
            OnDeadEvent.AddListener(AddExp);
            
            
            BehaviorGraphAgent.GetVariable("ChangeBossState",out BlackboardVariable<ChangeBossState> state);
            
            Debug.Assert(state != null, "Enemy has Not State Change");
            changeBossState = state;
            
        }
    
        private void OnDestroy()
        {
            OnHitEvent.RemoveListener(StartKnockback);
            OnHitEvent.RemoveListener(GeneratorText);
            OnDeadEvent.RemoveListener(AddExp);
        }

        private void AddExp()
        {
            Player.level.AddExp(Random.Range(minExp , maxExp));
        }
        
        private void GeneratorText(ActionData actionData)
        {
            Vector3 textPosition = actionData.hitPoint;
                        
            FloatingTextGenerator.Instance.GenerateText
            (
                Mathf.CeilToInt(actionData.damageAmount).ToString(),
                textPosition,
                actionData.textColor == default ? Color.white : actionData.textColor);
        }
        
        public override void TakeDamage(ActionData actionData)
        {
            if((isDead || !IsDamageTime()) && actionData.stun == false)return;
            
            lastDamageTime = Time.time; 
            
            currentHealth -= actionData.damageAmount;
            OnChangeHealthEvent?.Invoke(GetHealthPercent());
            
            if(actionData.stun)
                ChangeParryState();
            
            OnHitEvent?.Invoke(actionData);
                            
            if (currentHealth <= 0)
            {
                TriggerState(BossState.Dead);
                Dead();
            }
            
        }
                
        protected bool IsDamageTime()
        {
            return Time.time > lastDamageTime + DAMAGE_INTERVAL;
        }
        
        public void AddMaxHealth(float currentIndex)
        {
            maxHealth += currentIndex * 1.7f;
            currentHealth = maxHealth;
        }
        
        public override void TakeHeal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth , maxHealth);
        }
                
        protected void TriggerState(BossState state)
        {
            if(isDead)return;
            
            BehaviorGraphAgent.SetVariableValue("BossState", state);
            changeBossState.SendEventMessage(state);
        }
        
        private float GetHealthPercent()
        {
            return currentHealth / maxHealth;
        }
        
        public virtual void ChangeParryState()
        {
            animationController.StopAllAnimationEvents();
            TriggerState(BossState.Hurt);
        }
        
        private void StartKnockback(ActionData actionData)
        {
            if(actionData.knockbackDirection == default || actionData.knockbackForce == 0 || isKnockback)return;
            
            StartCoroutine(
                Knockback(actionData.knockbackDirection , actionData.knockbackForce));
        }
        
        private IEnumerator Knockback(Vector3 knockbackDirection, float knockbackForce)
        {
            if(navMeshAgent == null)yield break;
            
            isKnockback = true;
            
            navMeshAgent.enabled = false;
            enemyRigidbody.useGravity = true;
            enemyRigidbody.isKinematic = false;
            enemyRigidbody.freezeRotation = true;
    
            enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            
            yield return waitForFixedUpdate;
            
            if(navMeshAgent == null)yield break;
            
            float timeout = 0.5f; 
            float timer = 0f;
    
            while (enemyRigidbody.linearVelocity.sqrMagnitude > 0.01f && timer < timeout) 
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            if(navMeshAgent == null)yield break;
            
            enemyRigidbody.linearVelocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;
            
            yield return new WaitForFixedUpdate();
            
            if(navMeshAgent == null)yield break;
            
            transform.position = new Vector3(transform.position.x, navMeshAgent.nextPosition.y, transform.position.z);
            
            navMeshAgent.Warp(transform.position);
            
            enemyRigidbody.freezeRotation = false;
            navMeshAgent.enabled = true;
            enemyRigidbody.useGravity = false;
            enemyRigidbody.isKinematic = true;
            
            if (navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath();
            }

            isKnockback = false;
        }
    }
}
