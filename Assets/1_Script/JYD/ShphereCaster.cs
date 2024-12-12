using UnityEngine;

public class SphereCaster : LayerCaster
{
    [SerializeField][Range(0.5f, 3f)] private float _casterRadius = 1f;
    [SerializeField][Range(0f, 1f)] private float _casterInterpolation = 0.5f;
    [SerializeField][Range(0f, 3f)] private float _castingRange = 1f;
    
    public override bool CastDamage()
    {
        Vector3 startPos = GetStartPosition();
        
        bool isHit = Physics.SphereCast(
            startPos, 
            _casterRadius, 
            transform.forward, 
            out RaycastHit hit, 
            _castingRange, targetLayer);

        if(isHit)
        {
            //Debug.Log($"맞았습니다. {hit.collider.name}");
            OnCastDamageEvent?.Invoke();
            if(hit.collider.TryGetComponent(out IDamageble health))
            {
                float knockbackPower = 3f;
                
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
        }
        return isHit;
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