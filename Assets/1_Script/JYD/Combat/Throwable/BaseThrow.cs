using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseThrow : MonoBehaviour
    {
        [SerializeField] protected float forceAmount;
        
        [SerializeField] protected Rigidbody Rigidbody;
        
        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
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

            Rigidbody.mass = 1;
            Rigidbody.AddForce(force * forceAmount, ForceMode.Impulse);
        }
        
        protected virtual void SetRigid(bool active, float mass)
        {
            SetPhysicsState(active);
            Rigidbody.mass = mass;
        }
        
    }
}