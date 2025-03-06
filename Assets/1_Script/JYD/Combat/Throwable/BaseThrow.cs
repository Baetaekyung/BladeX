using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade.Combat.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseThrow : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;
        public float forceAmount;
        //protected NavMeshObstacle obstacle;

        [SerializeField] protected Rigidbody Rigidbody;

        protected virtual void Start()
        {
            //obstacle = GetComponent<NavMeshObstacle>();
            Rigidbody = GetComponent<Rigidbody>();
            //SetPhysicsState(true);
        }

        public virtual void SetPhysicsState(bool isActive)
        {
            //obstacle.enabled = false;
            
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

        public virtual void SetRigid(bool PS, float RM)
        {
            SetPhysicsState(PS);

            Rigidbody.mass = RM;
        }
    }
}