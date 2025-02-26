using System;
using Unity.Behavior;
using UnityEditor;
using UnityEngine;
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

        public static event System.Action OnAnyEnemyDead;

        protected virtual void Start()
        {
            currentHealth = maxHealth;

            BehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            BehaviorGraphAgent.GetVariable("ChangeBossState",out BlackboardVariable<ChangeBossState> state);

            if (state != null)
                changeBossState = state;
            else
            {
                Debug.LogError("Goblin Enemy has Not State Change");
            }
            
        }
        
        public virtual void TakeDamage(ActionData actionData)
        {
            if(isDead)return;

            currentHealth--;
            OnChangeHealthEvent?.Invoke(GetHealthPercent());
        
            if (currentHealth <= 0)
            {
                TriggerState(BossState.Dead);
                Dead();
                return;
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
            OnAnyEnemyDead?.Invoke();
        }
        
        protected void TriggerState(BossState state)
        {
            BehaviorGraphAgent.SetVariableValue("BossState", state);
            changeBossState.SendEventMessage(state);
        }
        
        protected float GetHealthPercent()
        {
            print("ÀÀ¾î¾ÆÀÕ");
            return currentHealth / maxHealth;
        }

        public void ChangeParryState()
        {
            TriggerState(BossState.Hurt);
        }
        
        
    }
}
