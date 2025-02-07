using System.Collections.Generic;
using Swift_Blade.Boss;
using UnityEngine;

[System.Serializable]
public struct SpawnInfo
{
    public BaseBoss enemy;
    public Transform spawnPosition;
    public int enemyCount;
}

namespace Swift_Blade.Level
{
    public class EnemySpawner : MonoBehaviour
    {
        public LevelClearEventSO levelEvent;
        public List<SpawnInfo> spawnEnemies;

        public int waveCount;
        private int enemyCount;
        private int enemyCounter;

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            if (waveCount >= spawnEnemies.Count)
            {
                Debug.Log("모든 웨이브를 클리어했습니다!");
                return;
            }

            enemyCount = 0; 
            enemyCounter = 0; 
            
            for (int i = 0; i < spawnEnemies[waveCount].enemyCount; i++)
            {
                BaseBoss newEnemy = Instantiate(spawnEnemies[waveCount].enemy);
                newEnemy.transform.position = spawnEnemies[waveCount].spawnPosition.position;
                newEnemy.SetOwner(this);
                ++enemyCount;
            }

            ++waveCount;
        }

        public void CheckSpawn()
        {
            ++enemyCounter;
            
            if (waveCount >= spawnEnemies.Count)
            {
                if (enemyCount == enemyCounter)
                {
                    levelEvent.LevelClearEvent?.Invoke();
                    Debug.Log("이 스테이지를 클리어했습니다!");
                }
            }
            else
            {
                if (enemyCount == enemyCounter)
                {
                    Spawn();
                }
            }
        }
    }
}