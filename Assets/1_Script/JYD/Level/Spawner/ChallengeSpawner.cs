using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using System.Collections;
using Swift_Blade.Enemy;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade.Level
{
    public class ChallengeSpawner : Spawner
    {
        private bool isClear = false;
        
        [SerializeField] private float wavePeriod;
        
        [Space]
        [SerializeField] private float endTimeSecond;
        private float endTimer;
        
        [Header("Spawn Position")]
        [SerializeField] private Transform[] spawnPosition;
        [SerializeField] private ChallengeStageUIView challengeStageUI;
        
        private ChallengeStageRemainTime challengeStageRemainTime;
        
        private readonly List<BaseEnemy> allEnemyList = new List<BaseEnemy>(40);
        
        private WaitForSeconds countdownWait;
        private WaitForSeconds wavePeriodWait;
        
        protected override void Start()
        {
            base.Start();
            
            challengeStageRemainTime = new ChallengeStageRemainTime();
            
            wavePeriodWait = new WaitForSeconds(wavePeriod);
            countdownWait = new WaitForSeconds(1f);
                        
            challengeStageUI.SetText(Mathf.FloorToInt(endTimeSecond));
            challengeStageRemainTime.SetRemainTime(endTimeSecond);
            
            StartCoroutine(CountdownCoroutine());
        }
        
        protected override IEnumerator Spawn()
        {
            while (isClear == false)
            {
                waveCount %= spawnEnemies.Count;
                
                SpawnInfos waves = spawnEnemies[waveCount++];
            
                for (int j = 0; j < waves.spawnInfos.Length; j++)
                {
                    yield return new WaitForSeconds(waves.spawnInfos[j].delay);
                                        
                    var enemyPrefab = waves.spawnInfos[j].enemy;
                    var newEnemy = Instantiate(enemyPrefab, spawnPosition[j].position, Quaternion.identity);
                    allEnemyList.Add(newEnemy);
                    
                    PlaySpawnParticle(newEnemy.transform.position);
                }
                
                yield return wavePeriodWait;
            }
        }
        
        private IEnumerator CountdownCoroutine()
        {
            while (!isClear)
            {
                yield return countdownWait;
                endTimer++;
                                
                challengeStageRemainTime.DecreaseRemainTime();
                challengeStageUI.SetText(challengeStageRemainTime.GetRemainTime());
                
                if (endTimer >= endTimeSecond)
                {
                    TimeOut();
                    yield break; 
                }
                
            }
        }
        
        private void TimeOut()
        {
            isClear = true;
            
            StopAllCoroutines();
            challengeStageUI.SetText();
                                    
            foreach (var enemy in allEnemyList)
            {
                if (enemy != null)
                {
                    if (enemy.TryGetComponent(out BaseEnemyHealth health) && !health.isDead )
                    {
                        ActionData actionData = new ActionData { damageAmount = 9999 };
                        health.TakeDamage(actionData);
                    }
                }
            }
            
            StartCoroutine(LevelClear());
        }
                
    }
}
