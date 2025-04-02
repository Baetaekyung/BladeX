using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVFXPlayer : MonoBehaviour,IEntityComponent
    {
        [SerializeField] private Transform playerTrm;
        [Space]
        [SerializeField] private PoolPrefabMonoBehaviourSO dustParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO hitSlashParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO parryParticle;
        [SerializeField] private Transform parryParticleTrm;
        [SerializeField] private PoolPrefabMonoBehaviourSO levelUpParticle;
        [SerializeField] private Transform levelUpEffectTrm;
        [SerializeField] private PoolPrefabMonoBehaviourSO healParticle;
        
        
        private void Start()
        {
            Player.LevelStat.OnLevelUp += LevelUpEffect;
        }

        private void OnDestroy()
        {
            Player.LevelStat.OnLevelUp -= LevelUpEffect;
        }

        public void EntityComponentAwake(Entity entity)
        {
            MonoGenericPool<DustParticle>.Initialize(dustParticle);
            MonoGenericPool<HitSlashParticle>.Initialize(hitSlashParticle);
            MonoGenericPool<ParryParticle>.Initialize(parryParticle);
            MonoGenericPool<LevelUpParticle>.Initialize(levelUpParticle);
            MonoGenericPool<PlayerHealParticle>.Initialize(healParticle);
        }
                
        public void PlayDamageEffect(ActionData actionData)
        {
            DustParticle dustParticle = MonoGenericPool<DustParticle>.Pop();
            dustParticle.transform.position = actionData.hitPoint;
            dustParticle.transform.rotation = Quaternion.LookRotation(-actionData.hitNormal);
            
            HitSlashParticle hitSlashParticle = MonoGenericPool<HitSlashParticle>.Pop();
            hitSlashParticle.transform.position = actionData.hitPoint;
            hitSlashParticle.transform.rotation = Quaternion.LookRotation(-actionData.hitNormal);
        }
        
        public void PlayParryEffect()
        {
            ParryParticle parryParticle = MonoGenericPool<ParryParticle>.Pop();
            parryParticle.transform.position = parryParticleTrm.position;
            
            HitSlashParticle hitSlashParticle = MonoGenericPool<HitSlashParticle>.Pop();
            hitSlashParticle.transform.position = parryParticleTrm.position;
        }

        private void LevelUpEffect(Player.LevelStat levelStat)
        {
            LevelUpParticle levelUpParticle = MonoGenericPool<LevelUpParticle>.Pop();
            levelUpParticle.transform.position = levelUpEffectTrm.position;
        }

        public void PlayHealEffect()
        {
            PlayerHealParticle healParticle = MonoGenericPool<PlayerHealParticle>.Pop();
            healParticle.transform.position = playerTrm.position + new Vector3(0,1,0);
            
        }
        
        
    }
}
