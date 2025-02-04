using System;
using System.Collections.Generic;
using DG.Tweening;
using Swift_Blade.Boss;
using UnityEngine;
using UnityEngine.Events;

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
        public List<SpawnInfo> spawnEnemies;
        private List<BaseBoss> currentEnemies;

        public int waveCount;

        private void Start()
        {
            currentEnemies = new List<BaseBoss>();
            Spawn();
        }

        public void Spawn()
        {
            if (waveCount >= spawnEnemies.Count)
            {
                Debug.Log("이 스테이지 클리어.");    
            }
            else
            {
                for (int i = 0; i <spawnEnemies[waveCount].enemyCount; i++ )
                {
                    BaseBoss newEnemy = Instantiate(spawnEnemies[waveCount].enemy);
                    currentEnemies.Add(newEnemy);
                    newEnemy.transform.position = spawnEnemies[waveCount].spawnPosition.position;
                    
                    
                }
                ++waveCount;
            }
        }
        
        public void Remove()
        {
            
        }
        
        
    }
}
