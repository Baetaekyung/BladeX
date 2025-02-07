using Swift_Blade.Combat.Projectile;
using UnityEngine;

namespace Swift_Blade.projectile
{
    public class GravityStone : Stone
    {
        [SerializeField] private float gravity = 9.8f;

        private Vector3 initialPosition;           
        private Vector3 initialVelocity;
        
        public void Update()
        {
            transform.rotation = Quaternion.LookRotation(Rigidbody.linearVelocity);
        }
        
        public void Fire(float fireAngle, Vector3 firePos, Vector3 targetPos)
        {
            transform.position = firePos;
                
            float angle = fireAngle * Mathf.Deg2Rad;
            Vector3 planeTarget = new Vector3(targetPos.x, 0, targetPos.z);
            Vector3 planePosition = new Vector3(firePos.x, 0, firePos.z);
            
            float distance = Vector3.Distance(planeTarget, planePosition);
            float yOffset = firePos.y - targetPos.y;
            
            float initVelocity = (1 / Mathf.Cos(angle))
                                 * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) 
                                              / (distance * Mathf.Tan(angle) + yOffset));
                        
            float yVelocity = initVelocity * Mathf.Sin(angle);
            float zVelocity = initVelocity * Mathf.Cos(angle);
            Vector3 velocity = new Vector3(0, yVelocity, zVelocity);
            
            Vector3 planeDirection = (planeTarget - planePosition).normalized;
            Quaternion rotation = Quaternion.LookRotation(planeDirection);
            
            Vector3 finalVelocity = rotation * velocity;
            
            if (Rigidbody == null)
            {
                Rigidbody = GetComponent<Rigidbody>();
            }
            
            finalVelocity.x /= 2;
            
            Rigidbody.linearVelocity = finalVelocity;
            Rigidbody.AddTorque(new Vector3(5f, 0, 0));
            
        }

    }
}