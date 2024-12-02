using Unity.Behavior;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour , IDamagable
{
    public float enemyHealth;
        
    public BehaviorGraphAgent BehaviorGraphAgent;
    public BossAnimationController BossAnimationController;
    [SerializeField] private ChangeState change;

    public Animator Animator;
    public bool isGuarding;
    public int maxGuardCount;
    private int guardCount;


    private void Start()
    {
        guardCount = maxGuardCount;
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
                BossAnimationController.GetKnockback();
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
                enemyHealth -= 10;
            }
            else
            {
                isGuarding = true;
                BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Guard);
                change.SendEventMessage(BossState.Guard);
                enemyHealth -= 5;
            }
        }

        if (enemyHealth <= 0)
        {
            BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Dead);
            change.SendEventMessage(BossState.Dead);
        }
        
        BossAnimationController.GetKnockback();
    }

    public void OffGuarding()
    {
        guardCount = maxGuardCount;
        isGuarding = false;
    }
    
    public void TakeHeal()
    {
        
    }

    public void Dead()
    {
        
    }
}