using System;
using UnityEngine;

namespace Swift_Blade.projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected LayerMask whatIsTarget;
        
        protected Rigidbody Rigidbody;
        protected float timer = 0;

        protected Vector3 direction;
        
         private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;
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
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if((whatIsTarget & (1 << other.gameObject.layer)) != 0)
                Destroy(gameObject);
        }

        public void SetDirection(Vector3 vector3)
        {
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;
            
            Rigidbody.AddForce(vector3 * 20);
        }
        
    }
}
