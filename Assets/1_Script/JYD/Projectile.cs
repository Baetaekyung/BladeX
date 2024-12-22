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
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                Destroy(gameObject);
            }
            
            Rigidbody.linearVelocity = transform.forward * moveSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if((whatIsTarget & (1 << other.gameObject.layer)) != 0)
                Destroy(gameObject);
        }
    }
}
