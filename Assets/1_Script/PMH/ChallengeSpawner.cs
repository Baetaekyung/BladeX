using System;
using System.Collections;
using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Enemy;
using Swift_Blade.Level;
using Swift_Blade.Level.Portal;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class ChallengeSpawner : MonoBehaviour
    {
        public PoolPrefabMonoBehaviourSO enemySpawnParticle;
        public SpawnInfos[] spawnEnemies;
        public List<BaseEnemy> allEnemyList;
        
        [SerializeField] private SceneManagerSO sceneManagerSO;
        [SerializeField] private NodeList nodeList;
        
        [SerializeField] private int waveCount;
        [SerializeField] private float wavePeriod;
        
        [Space]
        [SerializeField] private float endTimeSecond;
        private float endTimer;
        
        [SerializeField] private Transform[] spawnPosition;
        [SerializeField] private Transform[] portalTrm;
                
        private bool isGameEnd = false;
        
        private void Start()
        {
            MonoGenericPool<EnemySpawnParticle>.Initialize(enemySpawnParticle);
            
            StartCoroutine(EnemyWavesCoroutine());
            
        }

        private void Update()
        {
            endTimer += Time.deltaTime;
            if (endTimer >= endTimeSecond)
            {
                TimesOut();
                endTimer = 0;

                Node[] node = nodeList.GetNode();
                for (int i = 0; i < node.Length; i++)
                {
                     Portal portal = Instantiate(node[i].GetPortalPrefab() , portalTrm[i].position,Quaternion.identity);
                     portal.SetScene(node[i].nodeName);
                }
            }
        }

        private void TimesOut()
        {
            isGameEnd = true;
            StopAllCoroutines();
            
            foreach (var enemy in allEnemyList)
            {
                if (enemy != null)
                {
                    ActionData actionData = new ActionData { damageAmount = 9999 };
                    enemy.GetComponent<BaseEnemyHealth>().TakeDamage(actionData);
                }
            }
            
            sceneManagerSO.LevelClear();
        }

        private IEnumerator EnemyWavesCoroutine()
        {
            while (isGameEnd == false)
            {
                waveCount %= spawnEnemies.Length;
            
                SpawnInfos waves = spawnEnemies[waveCount++];
            
                for (int j = 0; j < waves.spawnInfos.Length; j++)
                {
                    yield return new WaitForSeconds(waves.spawnInfos[j].delay);
                    
                    var enemyPrefab = waves.spawnInfos[j].enemy;
                    
                    var nowEnemy = Instantiate(enemyPrefab, spawnPosition[j].position, Quaternion.identity);
                    allEnemyList.Add(nowEnemy);
                
                    EnemySpawnParticle spawnParticle = MonoGenericPool<EnemySpawnParticle>.Pop();
                    spawnParticle.transform.position = spawnPosition[j].position;
                    
                }
                
                yield return new WaitForSeconds(wavePeriod);
            }
            
            
        }
    }
}
