using System;
using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class PlayerVFXPlayer : MonoBehaviour,IEntityComponent
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO dustParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO hitSlashParticle;
        [SerializeField] private PoolPrefabMonoBehaviourSO parryParticle;
        [SerializeField] private Transform parryParticleTrm;
        [SerializeField] private PoolPrefabMonoBehaviourSO levelUpParticle;
        [SerializeField] private Transform levelUpEffectTrm;
        
        private PlayerStatCompo _statCompo;

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
            MonoGenericPool<Dust>.Initialize(dustParticle);
            MonoGenericPool<HitSlash>.Initialize(hitSlashParticle);
            MonoGenericPool<ParryParticle>.Initialize(parryParticle);
            MonoGenericPool<LevelUpParticle>.Initialize(levelUpParticle);
        }
                
        public void PlayDamageEffect(ActionData actionData)
        {
            Dust dust = MonoGenericPool<Dust>.Pop();
            dust.transform.position = actionData.hitPoint;
            dust.transform.rotation = Quaternion.LookRotation(-actionData.hitNormal);
            
            HitSlash hitSlash = MonoGenericPool<HitSlash>.Pop();
            hitSlash.transform.position = actionData.hitPoint;
            hitSlash.transform.rotation = Quaternion.LookRotation(-actionData.hitNormal);
        }
        
        public void PlayParryEffect()
        {
            ParryParticle parryParticle = MonoGenericPool<ParryParticle>.Pop();
            parryParticle.transform.position = parryParticleTrm.position;
            
            HitSlash hitSlash = MonoGenericPool<HitSlash>.Pop();
            hitSlash.transform.position = parryParticleTrm.position;
        }

        private void LevelUpEffect(Player.LevelStat levelStat)
        {
            LevelUpParticle levelUpParticle = MonoGenericPool<LevelUpParticle>.Pop();
            levelUpParticle.transform.position = levelUpEffectTrm.position;
            
        }
    }
}
