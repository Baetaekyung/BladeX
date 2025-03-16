using System;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public class Arrow : MonoBehaviour , IPoolable
    {
        [SerializeField] private float speed;
        [SerializeField] private float pushTime;
        private float pushTimer;

        private TrailRenderer trailRenderer;
        private Rigidbody rigidBody;
        
        private bool deadFlag;
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        private void Update()
        {
            pushTimer += Time.deltaTime;
            if (pushTimer > pushTime)
            {
                MonoGenericPool<Arrow>.Push(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (deadFlag) return;
            deadFlag = true;
            //do dead arrow stuff here
            
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(new ActionData() { damageAmount = 1, stun = false });
                MonoGenericPool<Arrow>.Push(this);
            }
        }
        
        public void OnPopInitialize()
        {
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.linearVelocity = Vector3.zero;
            trailRenderer.Clear();
            deadFlag = false;
            
            pushTimer = 0;
        }
        
        public void Shot()
        {
            Vector3 velocity = transform.forward;
            rigidBody.linearVelocity = velocity * speed;

        }

        public void OnPop()
        {
            
        }
    }
}
