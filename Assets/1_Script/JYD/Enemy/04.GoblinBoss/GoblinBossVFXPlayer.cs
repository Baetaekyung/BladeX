using System;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Enemy.Goblin
{
    public class GoblinBossVFXPlayer : MonoBehaviour
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO spawnEffect;

        private void Start()
        {
            MonoGenericPool<SmallSpawnParticle>.Initialize(spawnEffect);
        }

        public void PlaySpawnEffect(Vector3 spawnPosition)
        {
            SmallSpawnParticle smallSpawnParticle = MonoGenericPool<SmallSpawnParticle>.Pop();
            smallSpawnParticle.transform.position = spawnPosition;
        }
        
    }
}
