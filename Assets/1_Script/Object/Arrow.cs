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

        [SerializeField] private PoolPrefabMonoBehaviourSO dustParticle;
        
        private TrailRenderer trailRenderer;
        private Rigidbody rigidBody;
        private Vector3 originScale;
        
        private bool deadFlag;
        
        private const string enemyLayerName = "Enemy";
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
            originScale = transform.localScale;
            
            MonoGenericPool<DustParticle>.Initialize(dustParticle);
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
                        playerParryController.ParryEvents?.Invoke();
                        Reflection(other.GetComponentInParent<Player>().GetPlayerTransform);
                    }
                    else
                    {
                        Hit(health);
                    }
                    
                }
            }            
            
        }

        private void Hit(IDamageble health)
        {
            deadFlag = true;
            health.TakeDamage(new ActionData() { damageAmount = 1, stun = true });
                        
            MonoGenericPool<DustParticle>.Pop().transform.position = transform.position;
            MonoGenericPool<Arrow>.Push(this);
        }

        private void Reflection(Transform player)
        {
            transform.rotation = Quaternion.LookRotation(player.forward);
            rigidBody.linearVelocity = transform.forward * speed;
            
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
            transform.localScale = originScale;
            
            pushTimer = 0;
        }
    }
}
