using System;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour , IDamagable
{
    public float maxHealth;
    public float currentHealth;
    
    public BehaviorGraphAgent BehaviorGraphAgent;
    public BossAnimationController BossAnimationController;
    [SerializeField] private ChangeState change;

    public Animator Animator;
    public bool isGuarding;
    public int maxGuardCount;
    private int guardCount;

    public event Action<ActionData> OnHitEvent; 
    public event Action OnDeadEvent; 
    private void Start()
    {
        guardCount = maxGuardCount;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage();
        }
    }
    
    public void TakeDamage()
    {
        if (isGuarding)
        {
            guardCount--;
            if (guardCount > 0)
            {
                Animator.SetTrigger("GuardHit");
                Animator.SetInteger("GuardHitType",Random.Range(0,2));
            }
            else
            {
                Animator.SetTrigger("GuardBreak");
            }
        }
        else
        {
            float random = Random.value;
            
            if (random >= 0.3f)
            {
                BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Hurt);
                change.SendEventMessage(BossState.Hurt);
                currentHealth -= 10;
            }
            else
            {
                isGuarding = true;
                BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Guard);
                change.SendEventMessage(BossState.Guard);
                currentHealth -= 5;
            }
            OnHitEvent.Invoke(HealthPercent());
        }

        if (currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
        
    }

    public void OffGuarding()
    {
        guardCount = maxGuardCount;
        isGuarding = false;
    }

    private ActionData HealthPercent()
    {
        ActionData actionData = new ActionData();
        actionData.healthPercent = currentHealth / maxHealth;
        
        return actionData;
    }
    
    public void TakeHeal()
    {
        
    }

    public void Dead()
    {
        BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Dead);
        change.SendEventMessage(BossState.Dead);
    }
}