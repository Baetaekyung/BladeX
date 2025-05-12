using Swift_Blade.Audio;
using Swift_Blade.Combat;
using Swift_Blade.Combat.Health;
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
            if (deadFlag == false)
            {
                pushTimer += Time.deltaTime;
                if (pushTimer > pushTime)
                {
                    MonoGenericPool<Arrow>.Push(this);
                }
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
                    Vector3 hitPosition = other.ClosestPoint(transform.position);
                    PlayDustParticle(hitPosition);
                    
                    Hit(health);
                }
            }
            else
            {
                Vector3 hitPosition = other.ClosestPoint(transform.position);
                PlayDustParticle(hitPosition);
                                
                AudioManager.PlayWithInit(groundHitAudio.GetRandomAudio,true);
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
                Vector3 particlePosition = other.ClosestPoint(transform.position);
                PlayDustParticle(particlePosition);
                
                Hit(health);
            }
        }
        
        private void Hit(IHealth health)
        {
            health.TakeDamage(new ActionData()
            {
                damageAmount = health is PlayerHealth ? 1 : 4, 
                stun = true,
                hitPoint = transform.position + new Vector3(0,0.25f,0),
                textColor = Color.red
            });
            
            AudioManager.PlayWithInit(bodyHitAudio.GetRandomAudio,true);
            MonoGenericPool<Arrow>.Push(this);
        }
            
        private void PlayDustParticle(Vector3 particlePosition)
        {
            MonoGenericPool<DustParticle>.Pop().transform.position = particlePosition;
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

        public void OnPush()
        {
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.linearVelocity = Vector3.zero;
            transform.localScale = originScale;
            
            trailRenderer.Clear();
          
        }

        public void OnPop()
        {
            pushTimer = 0;
            deadFlag = false;
        }
    }
}
