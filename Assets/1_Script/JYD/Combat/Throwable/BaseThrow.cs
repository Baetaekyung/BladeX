using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    public class BaseThrow : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;
        public float forceAmount;
        
        protected Rigidbody Rigidbody;
        
        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

            //SetPhysicsState(true);
        }
        
        public virtual void SetPhysicsState(bool isActive)  
        {  
            Rigidbody.useGravity = !isActive;  
            Rigidbody.isKinematic = isActive;  
        }


        public virtual void SetDirection(Vector3 force)
        {
            transform.parent = null;
            
            SetPhysicsState(false);
            
            Rigidbody.AddForce(force * forceAmount,ForceMode.Impulse);
        }
    }
}
