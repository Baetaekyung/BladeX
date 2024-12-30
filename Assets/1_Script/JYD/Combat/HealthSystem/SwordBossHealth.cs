using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SwordBossHealth : MonoBehaviour , IDamageble
{
    public UnityEvent<ActionData> OnHitEvent;
    public UnityEvent<ActionData> OnParryHitEvent;
    public UnityEvent OnDeadEvent;
    
    public event Action<float> OnChangeHealthEvent; 
    
    public float maxHealth;
    public float currentHealth;
        
    [Space]
    [SerializeField] private BehaviorGraphAgent BehaviorGraphAgent; 
    [SerializeField] private ChangeBossState changeBossState;
    
    private void Start()
    {
        currentHealth = maxHealth;
                
    }

    private void OnDestroy()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ActionData action = new ActionData(Vector3.zero, 0.5f, 10f, 20f, transform, AttackType.Parry);
            TakeDamage(action);
        }
    }
    
    public void TakeDamage(ActionData actionData)
    {
        if (Random.value <= 0.1f)
        {
            TriggerState(BossState.Step);
            return;
        }
                
        currentHealth -= actionData.damageAmount;
        OnChangeHealthEvent?.Invoke(GetHealthPercent());
        
        if (currentHealth <= 0)
        {
            TriggerState(BossState.Dead);
            Dead();
            return;
        }

        if (actionData.attackType == AttackType.Parry)
        {
            TriggerState(BossState.Hurt);
            OnParryHitEvent?.Invoke(actionData);
        }
        
        OnHitEvent?.Invoke(actionData);
       
    }
    
    private void TriggerState(BossState state)
    {
        BehaviorGraphAgent.SetVariableValue("BossState", state);
        changeBossState.SendEventMessage(state);
    }

    private float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }

    public void TakeHeal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth , maxHealth);
    }

    public void Dead()
    {
        OnDeadEvent?.Invoke();
    }
    
   
}