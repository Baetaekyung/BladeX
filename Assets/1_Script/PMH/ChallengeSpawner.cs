using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using System.Collections;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade.Level
{
    public class ChallengeSpawner : MonoBehaviour
    {
        public PoolPrefabMonoBehaviourSO enemySpawnParticle;
        public SpawnInfos[] spawnEnemies;
        
        [SerializeField] private SceneManagerSO sceneManagerSO;
        [SerializeField] private NodeList nodeList;
        
        [SerializeField] private int waveCount;
        [SerializeField] private float wavePeriod;
        
        [Space]
        [SerializeField] private float endTimeSecond;
        private float endTimer;
        
        [SerializeField] private Transform[] spawnPosition;
        [SerializeField] private Transform[] portalTrm;
        
        [SerializeField] private ChallengeStageUIView challengeStageUI;
        private ChallengeStageRemainTime challengeStageRemainTime;
        
        public float dustParticleDelay;
        
        private List<BaseEnemy> allEnemyList = new List<BaseEnemy>();
        private bool isGameEnd = false;
        private WaitForSeconds countPeriod;
        
        private void Awake()
        {
            MonoGenericPool<DustUpParticle>.Initialize(enemySpawnParticle);
            
            challengeStageRemainTime = new ChallengeStageRemainTime();
            countPeriod = new WaitForSeconds(1f);
        }
        
        private void Start()
        {
            challengeStageRemainTime.SetRemainTime(endTimeSecond);
                                   
            StartCoroutine(EnemyWavesCoroutine());
            StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            while (!isGameEnd)
            {
                yield return countPeriod;
                
                challengeStageRemainTime.DecreaseRemainTime();
                challengeStageUI.SetText(challengeStageRemainTime.GetRemainTime());
                
                if (endTimer >= endTimeSecond)
                {
                    
                    yield break; 
                }
            }
            
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
            challengeStageUI.SetText();
            
            isGameEnd = true;
                        
            foreach (var enemy in allEnemyList)
            {
                if (enemy != null)
                {
                    ActionData actionData = new ActionData { damageAmount = 9999 };
                    enemy.GetComponent<BaseEnemyHealth>().TakeDamage(actionData);
                }
            }

            StartCoroutine(LevelClear());
        }
                
        private IEnumerator LevelClear()
        {
            sceneManagerSO.LevelClear();

            Node[] newNode = nodeList.GetNodes();

            yield return new WaitForSeconds(dustParticleDelay);

            for (int i = 0; i < newNode.Length; ++i)
            {
                var doorPosition = spawnPosition[i].position;
                
                DustUpParticle dustUpParticle = MonoGenericPool<DustUpParticle>.Pop();
                dustUpParticle.transform.position = doorPosition;

                Door newDoor = Instantiate(newNode[i].GetPortalPrefab(), doorPosition, Quaternion.identity);
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
                    
                    DustUpParticle spawnParticle = MonoGenericPool<DustUpParticle>.Pop();
                    spawnParticle.transform.position = spawnPosition[j].position;
                    
                }
                
                yield return new WaitForSeconds(wavePeriod);
            }
            
        }
        
    }
}
