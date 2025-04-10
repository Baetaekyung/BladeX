using Swift_Blade.Audio;
using Swift_Blade.Combat;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public class Arrow : MonoBehaviour , IPoolable
    {
        [SerializeField] private LayerMask whatIsTarget;
        
        [SerializeField] private float speed;
        [SerializeField] private float pushTime;
        private float pushTimer;

        [SerializeField] private PoolPrefabMonoBehaviourSO dustParticle;

        [Header("Audio info")] 
        [SerializeField] private AudioCollectionSO groundHitAudio;
        [SerializeField] private AudioCollectionSO bodyHitAudio;
        
        
        private TrailRenderer trailRenderer;
        private Rigidbody rigidBody;
        private Vector3 originScale;
        
        private bool deadFlag;
        
        
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
            if(deadFlag)return;
            deadFlag = true;
                        
            if ((whatIsTarget & (1 << other.gameObject.layer)) != 0)
            {
                if (other.gameObject.TryGetComponent(out IHealth health))
                {
                    TryParry(other, health);
                }
                else
                {
                    Hit(health);
                }
            }
            else
            {
                AudioManager.PlayWithInit(groundHitAudio.GetRandomAudio,true);
                
                MonoGenericPool<DustParticle>.Pop().transform.position = transform.position;
                MonoGenericPool<Arrow>.Push(this);
            }
        }

        private void TryParry(Collider other, IHealth health)
        {
            if (other.TryGetComponent(out PlayerParryController playerParryController) && playerParryController.GetParry())
            {
                playerParryController.ParryEvents?.Invoke();
                Reflection(other.GetComponentInParent<Player>().GetPlayerTransform);
            }
            else
            {
                Hit(health);
            }
        }

        private void Hit(IHealth health)
        {
            health.TakeDamage(new ActionData() { damageAmount = 1, stun = true });
            
            AudioManager.PlayWithInit(bodyHitAudio.GetRandomAudio,true);
            
            MonoGenericPool<DustParticle>.Pop().transform.position = transform.position;
            MonoGenericPool<Arrow>.Push(this);
            
        }
    
        private void Reflection(Transform player)
        {
            deadFlag = false;
            
            transform.rotation = Quaternion.LookRotation(player.forward);
            rigidBody.linearVelocity = transform.forward * speed;
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
            transform.localScale = originScale;
                        
            pushTimer = 0;
        }
    }
}
