using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    public class Stone : MonoBehaviour,Throwable
    {
        [SerializeField] protected float moveSpeed;
        public LayerMask whatIsTarget;
        
        protected Rigidbody Rigidbody;
        
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

            SetPhysicsState(true);
        }
        
        public void SetPhysicsState(bool isActive)  
        {  
            Rigidbody.useGravity = !isActive;  
            Rigidbody.isKinematic = isActive;  
        }


        public void SetDirection(Vector3 force)
        {
            transform.parent = null;

            SetPhysicsState(false);
            
            Rigidbody.AddForce(force * 20,ForceMode.Impulse);
        }
    }
}
