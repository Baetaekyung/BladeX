using System;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public class Arrow : MonoBehaviour , IPoolable
    {
        [SerializeField] private float speed;
        [SerializeField] private float pushTime;
        private float pushTimer;
        
        private Rigidbody rigidBody;
        private bool deadFlag;
        private void Awake()
        {
            Vector3 velocity = transform.forward;
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.linearVelocity = velocity * speed;
        }

        private void Update()
        {
            pushTimer += Time.deltaTime;
            if (pushTimer > pushTime)
            {
                MonoGenericPool<Arrow>.Push(this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (deadFlag) return;
            deadFlag = true;
            //do dead arrow stuff here
            
            if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(new ActionData() { damageAmount = 1, stun = false });
            }
        }

        public void OnPopInitialize()
        {
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
            pushTimer = 0;
        }
                
    }
}
