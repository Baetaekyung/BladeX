using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade.Combat.Caster
{
    public abstract class LayerCaster : MonoBehaviour, ICasterAble
    {
        [SerializeField] [Range(0.5f, 10f)] protected float _casterRadius = 1f;
        [SerializeField] [Range(0f, 10f)] protected float _casterInterpolation = 0.5f;
        [SerializeField] [Range(0f, 10f)] protected float _castingRange = 1f;
        
        [FormerlySerializedAs("targetLayer")] public LayerMask whatIsTarget;
        public LayerMask whatIsObstacle;
        public UnityEvent<ActionData> OnCastDamageEvent;
        public UnityEvent OnCastEvent;
        public abstract bool Cast();
       
        protected virtual void ApplyDamage(IHealth health,ActionData actionData)
        {
            OnCastDamageEvent?.Invoke(actionData);
            health.TakeDamage(actionData);
        }
        
        protected Vector3 GetStartPosition()
        {
            return transform.position + transform.forward * -_casterInterpolation * 2;
        }

        protected bool IsNotObstacleLine()
        {
            Vector3 direction = transform.forward;
            Vector3 start = GetStartPosition() + new Vector3(0, 0.25f, 0);
            float distance = _castingRange;

            if (Physics.Raycast(start, direction, distance, whatIsObstacle))
            {
                return false;
            }

            return true;
        }
        
        
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetStartPosition(), _casterRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetStartPosition() + transform.forward * _castingRange, _casterRadius);
            Gizmos.color = Color.white;
        }
    }
}