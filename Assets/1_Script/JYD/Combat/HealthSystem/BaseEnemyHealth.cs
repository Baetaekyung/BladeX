using System.Collections;
using UnityEngine.AI;
using Unity.Behavior;
using UnityEngine;
using System;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;

namespace Swift_Blade.Combat.Health
{
    public class BaseEnemyHealth : BaseEntityHealth, IHealth
    {
        public event Action<float> OnChangeHealthEvent; 
        public float currentHealth;
        
        [Space]
        [SerializeField] protected BehaviorGraphAgent BehaviorGraphAgent;
        [SerializeField] protected ChangeBossState changeBossState;

        private BaseEnemyAnimationController animationController;
        private Rigidbody enemyRigidbody;
        private NavMeshAgent navMeshAgent;
        
        [Header("Knockback info")]
        public bool isKnockback = false;
        
        protected virtual void Start()
        {
            currentHealth = maxHealth;
            
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            BehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            animationController = GetComponentInChildren<BaseEnemyAnimationController>();
            
            OnHitEvent.AddListener(StartKnockback);
            
            BehaviorGraphAgent.GetVariable("ChangeBossState",out BlackboardVariable<ChangeBossState> state);
            
            if (state != null)
                changeBossState = state;
            else
            {
                Debug.LogError("Enemy has Not State Change");
            }
            
        }

        private void OnDestroy()
        {
            OnHitEvent.RemoveListener(StartKnockback);
        }
        
        public override void TakeDamage(ActionData actionData)
        {
            if(isDead)return;
            
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

        public override void TakeHeal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth , maxHealth);
        }
                
        private void TriggerState(BossState state)
        {
            if(isDead)return;
            
            BehaviorGraphAgent.SetVariableValue("BossState", state);
            changeBossState.SendEventMessage(state);
        }
        
        private float GetHealthPercent()
        {
            return currentHealth / maxHealth;
        }

        public void ChangeParryState()
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
            isKnockback = true;
            
            navMeshAgent.enabled = false;
            enemyRigidbody.useGravity = true;
            enemyRigidbody.isKinematic = false;
            enemyRigidbody.freezeRotation = true;
    
            enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            
            yield return new WaitForFixedUpdate();
    
            float timeout = 0.5f; 
            float timer = 0f;
    
            while (enemyRigidbody.linearVelocity.sqrMagnitude > 0.01f && timer < timeout) 
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            enemyRigidbody.linearVelocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;
            
            yield return new WaitForFixedUpdate();
    
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
