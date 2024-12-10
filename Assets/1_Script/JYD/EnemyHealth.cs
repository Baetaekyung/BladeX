using System;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour , IDamageble
{
    public float maxHealth;
    public float currentHealth;
    
    [Header("Groggy info")]
    public float maxGrogging = 10;
    public float curGrogging = 0;
    
    public BehaviorGraphAgent BehaviorGraphAgent;
    
    [Header("Animation info")]
    public BossAnimationController BossAnimationController;
    public Animator Animator;
    [SerializeField] private ChangeState change;
    
    [Header("Guard info")]
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
        ++curGrogging;
        
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
            OnHitEvent.Invoke(GetHealthPercent());
                       
        }
        
        if (currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }

        if (curGrogging >= maxGrogging)
        {
            curGrogging = 0;
            
            BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Groggy);
            change.SendEventMessage(BossState.Groggy);
        }
        
        
    }

    public void OffGuarding()
    {
        guardCount = maxGuardCount;
        isGuarding = false;
    }

    private ActionData GetHealthPercent()
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