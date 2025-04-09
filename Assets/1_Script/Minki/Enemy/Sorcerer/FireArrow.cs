using UnityEngine;

namespace Swift_Blade.Pool
{
    public class FireArrow : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsTarget;
        
        [SerializeField] private float speed;
        [SerializeField] private float pushTime;
        private float pushTimer;
        
        private Rigidbody rigidBody;
        
        private bool deadFlag;
        
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();

            Shot();
        }
        
        private void Update()
        {
            pushTimer += Time.deltaTime;
            if (pushTimer > pushTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((whatIsTarget & (1 << other.gameObject.layer)) != 0 && deadFlag == false)
            {
                if (other.gameObject.TryGetComponent(out IHealth health))
                {
                    Hit(health);
                }
            }
            else
            {
                deadFlag = true;
            }
        }

        private void Hit(IHealth health)
        {
            deadFlag = true;
            health.TakeDamage(new ActionData() { damageAmount = 1, stun = true });
        }

        public void Shot()
        {
            Vector3 velocity = transform.forward;
            rigidBody.linearVelocity = velocity * speed;
        }
    }
}
