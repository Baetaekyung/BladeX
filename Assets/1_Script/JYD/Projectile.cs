using System;
using UnityEngine;

namespace Swift_Blade
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private LayerMask whatIsTarget;
        
        private Rigidbody Rigidbody;
        private float timer = 0;
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
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
