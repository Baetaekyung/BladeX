using UnityEngine;

namespace Swift_Blade.Pool
{
    public class Arrow : MonoBehaviour , IPoolable
    {
        [SerializeField] private LayerMask whatIsObstacle;
        
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
            if ((whatIsObstacle & (1 << other.gameObject.layer)) != 0 && deadFlag == false)
            {
                if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                {
                    deadFlag = true;
                    playerHealth.TakeDamage(new ActionData() { damageAmount = 1, stun = true });
                    MonoGenericPool<Arrow>.Push(this);
                }
            }            
            
        }
                
        public void Shot()
        {
            Vector3 velocity = transform.forward;
            rigidBody.linearVelocity = velocity * speed;
        }

        public void OnPop()
        {
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.linearVelocity = Vector3.zero;
            trailRenderer.Clear();
            deadFlag = false;
            
            pushTimer = 0;
        }
    }
}
