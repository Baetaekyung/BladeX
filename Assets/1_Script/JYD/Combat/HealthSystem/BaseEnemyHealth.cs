using System;
using System.Collections;
using Swift_Blade.Enemy;
using Unity.Behavior;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Swift_Blade.Combat.Health
{
    public class BaseEnemyHealth : MonoBehaviour,IDamageble
    {
        public UnityEvent<ActionData> OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        public Action<float> OnChangeHealthEvent; 
        
        public float maxHealth;
        public float currentHealth;
        public bool isDead = false;
        
        [Space]
        [SerializeField] protected BehaviorGraphAgent BehaviorGraphAgent;
        [SerializeField] protected ChangeBossState changeBossState;

        public bool isKnockback = false;
        private Rigidbody enemyRigidbody;
        private NavMeshAgent navMeshAgent;
                
        protected virtual void Start()
        {
            currentHealth = maxHealth;

            navMeshAgent = GetComponent<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            BehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            
            BehaviorGraphAgent.GetVariable("ChangeBossState",out BlackboardVariable<ChangeBossState> state);

            if (state != null)
                changeBossState = state;
            else
            {
                Debug.LogError("Goblin Enemy has Not State Change");
            }

            OnHitEvent.AddListener(StartKnockback);
        }

        private void OnDestroy()
        {
            OnHitEvent.RemoveListener(StartKnockback);
        }

        public virtual void TakeDamage(ActionData actionData)
        {
            if(isDead)return;
            
            currentHealth -= actionData.damageAmount;
            OnChangeHealthEvent?.Invoke(GetHealthPercent());
            
            if(actionData.stun)
                ChangeParryState();
            
            if (currentHealth <= 0)
            {
                TriggerState(BossState.Dead);
                Dead();
            }
            
            OnHitEvent?.Invoke(actionData);
        }

        public virtual void TakeHeal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth , maxHealth);
        }

        public virtual void Dead()
        {
            isDead = true;
            OnDeadEvent?.Invoke();
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
