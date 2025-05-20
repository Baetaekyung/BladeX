using Swift_Blade.Audio;
using Swift_Blade.Combat.Health;
using Swift_Blade.Feeling;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Projectile
{
    public class Bomb : BaseThrow
    {
        public LayerMask whatIsTarget;
        public CameraShakeType shakeType;
        public PoolPrefabMonoBehaviourSO explosionSO;

        public float explosionRadius;
        public int enemyDamage = 5;
        
        private bool canExplosion;
        private bool hasExploded; // 무한루프 방지용 플래그
        private readonly Collider[] targets = new Collider[10];
        
        private LayerMask whatIsEnemy;

        [Space]
        [SerializeField] private AudioCollectionSO explosionSound;
        
        protected override void Start()
        {
            base.Start();
            whatIsEnemy = LayerMask.NameToLayer("Enemy");
                        
            MonoGenericPool<ExplosionParticle>.Initialize(explosionSO);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (CheckEnemyLayer(other) && canExplosion)
            {
                Explosion(other.GetContact(0).point);
            }
        }

        
        private bool CheckEnemyLayer(Collision other)
        {
            return other.gameObject.layer != whatIsEnemy;
        }
        private void Explosion(Vector3 explosionPoint)
        {
            if (canExplosion == false || hasExploded) 
                return;

            AudioManager.PlayWithInit(explosionSound.GetRandomAudio, true);
            
            canExplosion = false;
            hasExploded = true;
            
            int counts = Physics.OverlapSphereNonAlloc(explosionPoint, explosionRadius, targets, whatIsTarget);
            
            for (int i = 0; i < counts; i++)
            {
                var target = targets[i].GetComponentInChildren<IHealth>();
                if (target != null)
                {
                    var actionData = new ActionData();
                    actionData.damageAmount = target is BaseEnemyHealth ? enemyDamage : 1;
                    actionData.hitPoint = targets[i].transform.position + new Vector3(0, 0.25f, 0);
                    actionData.textColor = Color.yellow;
                    actionData.stun = true;
                    actionData.hurtType = 1;
                    
                    target.TakeDamage(actionData);
                }
                else if (targets[i].TryGetComponent(out Bomb otherBomb))
                {
                    otherBomb.SetPhysicsState(false);
                    otherBomb.SetDirection(Vector3.zero);
                    otherBomb.Explosion(otherBomb.transform.position);
                }
            }

            CameraShakeManager.Instance.DoShake(shakeType);

            var e = MonoGenericPool<ExplosionParticle>.Pop();
            e.transform.position = explosionPoint;
            
            Destroy(gameObject);
        }

        public override void SetPhysicsState(bool isActive)
        {
            base.SetPhysicsState(isActive);
            if (isActive == false)
                canExplosion = true;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
