using UnityEngine;

namespace Swift_Blade.projectile
{
    public class GravityProjectile : Projectile
    {
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private float airTime = 1f;

        private float elapsedTime = 0f;
        private Vector3 velocity;
        private Vector3 direction;
        
        public override void Update()
        {
            base.Update();

            transform.position = direction;

            /*elapsedTime += Time.deltaTime;

            if (elapsedTime < airTime)
            {
                velocity.y = gravity * (airTime - elapsedTime);
            }
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }

            Vector3 movement = direction;
            movement.y = 0;

            transform.position += (movement + velocity) * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(movement + velocity);*/
        }

        public void SetDirection(Vector3 _direction)
        {
            direction = _direction;
        }
    }
}