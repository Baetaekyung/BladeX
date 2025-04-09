using UnityEngine;

namespace Swift_Blade.Pool
{
    public class WindProjectileParticle : ParticlePoolAble<WindProjectileParticle>
    {
        private Vector3 direction;
        private Rigidbody windRigidbody;
        [SerializeField] private float speed;
                
        public override void OnPop()
        {
            base.OnPop();
            if(windRigidbody == null)
                windRigidbody = GetComponent<Rigidbody>();
            
            windRigidbody.linearVelocity = Vector3.zero;
            windRigidbody.angularVelocity = Vector3.zero;
        }
        
        protected override void Update()
        {
            base.Update();
            GetComponent<Rigidbody>().linearVelocity = direction * speed;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHealth health))
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
