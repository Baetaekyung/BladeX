using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Level.Door;
using System.Collections;
using Swift_Blade.Enemy;
using Swift_Blade.Level;
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
            if(isGameEnd == false)
                endTimer += Time.deltaTime;
            
            if (endTimer >= endTimeSecond)
            {
                endTimer = 0;
                TimeOut();
            }
        }

        private void TimeOut()
        {
            StopAllCoroutines();
            
            isGameEnd = true;
            
            foreach (var enemy in allEnemyList)
            {
                if (enemy != null)
                {
                    ActionData actionData = new ActionData { damageAmount = 9999 };
                    enemy.GetComponent<BaseEnemyHealth>().TakeDamage(actionData);
                }
            }

            LevelClear();
        }

        
        private void LevelClear()
        {
            sceneManagerSO.LevelClear();
            
            Node[] newNode = nodeList.GetNode();
    
            for (int i = 0; i < newNode.Length; ++i)
            {
                Door newDoor = Instantiate(newNode[i].GetPortalPrefab(), portalTrm[i].position, Quaternion.identity);
                newDoor.SetScene(newNode[i].nodeName);
                newDoor.UpDoor();
            }
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
