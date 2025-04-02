using Swift_Blade.Combat;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Pool
{
    public class Arrow : MonoBehaviour , IPoolable
    {
        [SerializeField] private LayerMask whatIsTarget;
        
        [SerializeField] private float speed;
        [SerializeField] private float pushTime;
        private float pushTimer;
        
        private TrailRenderer trailRenderer;
        private Rigidbody rigidBody;
        
        private bool deadFlag;

        private const string enemyLayerName = "Enemy";
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
            if ((whatIsTarget & (1 << other.gameObject.layer)) != 0 && deadFlag == false)
            {
                if (other.gameObject.TryGetComponent(out IDamageble health))
                {
                    if (other.TryGetComponent(out PlayerParryController playerParryController) && playerParryController.CanParry())
                    {
                        playerParryController.ParryEvents.Invoke();
                        Reflection();
                    }
                    else
                    {
                        deadFlag = true;
                        health.TakeDamage(new ActionData() { damageAmount = 1, stun = true });
                        MonoGenericPool<Arrow>.Push(this);
                    }
                    
                }
            }            
            
        }

        private void Reflection()
        {
            Vector3 direction = gameObject.transform.forward;
            direction = new Vector3(-direction.x, direction.y, -direction.z);
            rigidBody.linearVelocity = direction * speed;
            
            whatIsTarget |= (1 << LayerMask.NameToLayer(enemyLayerName));
            
            transform.localScale *= 1.5f;
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
            whatIsTarget &= ~LayerMask.GetMask(enemyLayerName);
            
            
            pushTimer = 0;
        }
    }
}
