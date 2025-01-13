using Swift_Blade;
using Swift_Blade.FSM.States;
using UnityEngine;
using UnityEngine.Events;

public class SphereCaster : LayerCaster
{
    [SerializeField][Range(0.5f, 10f)] private float _casterRadius = 1f;
    [SerializeField][Range(0f, 10f)] private float _casterInterpolation = 0.5f;
    [SerializeField][Range(0f, 10f)] private float _castingRange = 1f;

    public UnityEvent parryEvents;
    
    
    public override bool CastDamage()
    {
        Vector3 startPos = GetStartPosition();
        
        bool isHit = Physics.SphereCast(
            startPos, 
            _casterRadius, 
            transform.forward, 
            out RaycastHit hit, 
            _castingRange, targetLayer);

        if (isHit && hit.collider.TryGetComponent(out IDamageble health))
        {
            if (health is PlayerHealth playerHealth)
            {
                if (playerHealth.GetCurrentState() == PlayerStateEnum.Parry)
                {
                    parryEvents?.Invoke();
                    print("응어아잇");
                }
                else
                {
                    ApplyDamage(health);
                }
            }
            else
            {
                ApplyDamage(health);
            }
        }

        

        return isHit;
    }

    void ApplyDamage(IDamageble health)
    {
        OnCastDamageEvent?.Invoke();

        ActionData actionData = new ActionData
        {
            damageAmount = 10,
            knockbackDir = transform.forward,
            knockbackDuration = 0.2f,
            knockbackPower = 5,
            dealer = transform
        };

        health.TakeDamage(actionData);
    }
    
    public Vector3 GetStartPosition()
    {
        return transform.position + transform.forward * -_casterInterpolation * 2; 
    }
    
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetStartPosition(), _casterRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetStartPosition() + transform.forward * _castingRange, _casterRadius);
        Gizmos.color = Color.white;
        
    }
}