using System;
using System.Collections;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;
using Action = System.Action;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour , IDamageble
{
    public float maxHealth;
    public float currentHealth;
    
    public BehaviorGraphAgent BehaviorGraphAgent;
    
    [Header("Animation info")]
    public BossAnimationController BossAnimationController;
    public Animator Animator;
    
    public event Action<float> OnHitEvent;
    public event Action OnDeadEvent;

    [SerializeField] private Material _flashMat;
    [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
    private Material[] _originMats;
    
    private void Start()
    {
        currentHealth = maxHealth;
        
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _originMats = new Material[_meshRenderers.Length];
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _originMats[i] = _meshRenderers[i].material;
        }


        OnHitEvent += FlashMat;
    }

    private void OnDestroy()
    {
        OnHitEvent -= FlashMat;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ActionData actionData = new ActionData();
            actionData.damageAmount = 10;
            
            TakeDamage(actionData);
        }
    }
    
    public void TakeDamage(ActionData actionData)
    {
        if (currentHealth <= 0)
        {
            TriggerState(BossState.Dead , 0);
            OnDeadEvent?.Invoke();
            return;
        }
        
        HandleNonGuard(actionData.damageAmount);
       
    }

    private void HandleNonGuard(float damage)
    {
        TriggerState(BossState.Hurt, damage);
        OnHitEvent.Invoke(GetHealthPercent());
    }

    private void TriggerState(BossState state, float damage)
    {
        BehaviorGraphAgent.SetVariableValue("BossState", state);
        //changeBoss.SendEventMessage(state);
        currentHealth -= damage;
    }

    private void TriggerGroggyState()
    {
        TriggerState(BossState.Groggy, 0);
    }

    

    private float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
    
    public void TakeHeal()
    {
        
    }

    public void Dead()
    {
        BehaviorGraphAgent.SetVariableValue<BossState>("BossState", BossState.Dead);
        //changeBoss.SendEventMessage(BossState.Dead);
    }

    private void FlashMat(float _trash)
    {
        StartCoroutine(FlashRoutine());
    }
    
    private IEnumerator FlashRoutine()
    {
        foreach (var t in _meshRenderers)
        {
            t.material = _flashMat;
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material = _originMats[i];
        }
        
        
    }
}