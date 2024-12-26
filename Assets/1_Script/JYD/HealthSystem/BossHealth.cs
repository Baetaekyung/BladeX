using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;

public class BossHealth : MonoBehaviour , IDamageble
{
    public event Action<ActionData> OnHitEvent;
    public event Action<ActionData> OnParryHitEvent;
    public event Action OnDeadEvent;
    public event Action<float> OnChangeHealthEvent; 
    
    public float maxHealth;
    public float currentHealth;
        
    
    [Header("Animation info")]
    [SerializeField] private BossAnimationController BossAnimationController;
    [SerializeField] private Animator Animator;
     
    [Space]
    [SerializeField] private BehaviorGraphAgent BehaviorGraphAgent; 
    [SerializeField] private ChangeBossState changeBossState;
        
    [Header("Flash info")]
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
            ActionData action = new ActionData(Vector3.zero, 0.5f, 10f, 20f, transform, AttackType.Parry);
            TakeDamage(action);
        }
    }
    
    public void TakeDamage(ActionData actionData)
    {
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
    
    public void TakeHeal()
    {
        
    }

    public void Dead()
    {
        OnDeadEvent?.Invoke();
    }

    private void FlashMat(ActionData actionData)
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