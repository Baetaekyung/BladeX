using System;
using UnityEngine;

namespace Swift_Blade.projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected LayerMask whatIsTarget;
        [SerializeField] protected LayerMask whatIsGround;
        protected Rigidbody Rigidbody;
        protected float timer = 0;

        protected Vector3 direction;
        
         private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

            SetPhysicsState(true);
        }

        public virtual void Update()
        {
            /*timer += Time.deltaTime;
            if (timer >= 5)
            {
                Destroy(gameObject);
            }
            
            Rigidbody.linearVelocity = transform.forward * moveSpeed;*/
        }
        
        protected virtual void OnCollisionEnter(Collision other)
        {
                                        
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
