using System;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public class WindProjectileParticle : ParticlePoolAble<WindProjectileParticle>
    {
        private Vector3 direction;
        private Rigidbody rigidbody;
        [SerializeField] private float speed;
        
        

        public override void OnPop()
        {
            base.OnPop();
            if(rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
                
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        protected override void Update()
        {
            base.Update();
            rigidbody.linearVelocity = direction * speed;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageble health))
            {
                ActionData actionData = new ActionData();
                actionData.stun = true;
                actionData.damageAmount = 1;
                health.TakeDamage(actionData);
            }
        }

        public void SetDirection(Vector3 _direction)
        {
            direction = _direction;
            transform.rotation = Quaternion.LookRotation(_direction);
        }
    }
}
