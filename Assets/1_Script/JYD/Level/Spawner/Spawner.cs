using System.Collections.Generic;
using System.Collections;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using UnityEngine;
using System;

namespace Swift_Blade.Level
{
    [Serializable]
    public struct SpawnInfo
    {
        public BaseEnemy enemy;
        public float delay;
        public Transform spawnPosition;
    }

    [Serializable]
    public struct SpawnInfos
    {
        public SpawnInfo[] spawnInfos;
    }
    
    public abstract class Spawner : MonoBehaviour
    {
        public PoolPrefabMonoBehaviourSO enemySpawnParticle;
        public PoolPrefabMonoBehaviourSO doorSpawnParticle;
        public SceneManagerSO sceneManager;
        
        [Space]
        public List<SpawnInfos> spawnEnemies;
        
        [Header("Door info")]
        public float dustParticleDelay;
        public Transform[] doorTrm;

        private WaitForSeconds doorSpawnDelay;
        private bool isSpawnSmall;
        
        [Header("Wave Count")]
        public int waveCount;
            
        protected virtual void Start()
        {
            if (enemySpawnParticle.GetMono as EnemySpawnParticle)
            {
                MonoGenericPool<EnemySpawnParticle>.Initialize(enemySpawnParticle);
                isSpawnSmall = false;
            }
            else if(enemySpawnParticle.GetMono as SmallSpawnParticle)
            {
                MonoGenericPool<SmallSpawnParticle>.Initialize(enemySpawnParticle);
                isSpawnSmall = true;
            }
            
            MonoGenericPool<DustUpParticle>.Initialize(doorSpawnParticle);
            doorSpawnDelay = new WaitForSeconds(dustParticleDelay);
            
            StartCoroutine(Spawn());
        }

        protected void PlaySpawnParticle(Vector3 position)
        {
            if (isSpawnSmall)
            {
                SmallSpawnParticle spawnParticle = MonoGenericPool<SmallSpawnParticle>.Pop();
                spawnParticle.transform.position = position;
            }
            else
            {
                EnemySpawnParticle spawnParticle = MonoGenericPool<EnemySpawnParticle>.Pop();
                spawnParticle.transform.position = position;
            }
        }
        protected IEnumerator LevelClear()
        {
            sceneManager.LevelClear();
            
            Node[] newNode = sceneManager.GetNodeList().GetNodes();
            
            yield return doorSpawnDelay;
            
            for (int i = 0; i < newNode.Length; ++i)
            {
                var doorPosition = doorTrm[i].position;
                                
                DustUpParticle dustUpParticle = MonoGenericPool<DustUpParticle>.Pop();
                dustUpParticle.transform.position = doorPosition;
                                
                Door newDoor = Instantiate(newNode[i].GetPortalPrefab(), doorPosition, Quaternion.identity);
                newDoor.SetScene(newNode[i].nodeName);
                newDoor.UpDoor();
            }
        }
        
        protected abstract IEnumerator Spawn();
        
    }
}
