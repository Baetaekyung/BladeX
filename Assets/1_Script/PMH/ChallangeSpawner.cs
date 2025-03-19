using System.Collections;
using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class ChallangeSpawner : MonoBehaviour
    {
        public PoolPrefabMonoBehaviourSO enemySpawnParticle;
        private ActionData actionData;
        [SerializeField] private SceneManagerSO sceneManagerSO;
        public List<SpawnInfos> spawnEnemies;

        public List<BaseEnemy> spawnEnemyList;

        [SerializeField] private int waveCount;
        [SerializeField] private float waveCooltime;

        [SerializeField] private float endTimeSecond;

        [SerializeField] private Transform[] spawnPosition;

        private bool isGameEnd = false;

        private void Awake()
        {
            actionData.damageAmount = 999;
            actionData.dealer = default;
            actionData.hitNormal = default;
        }
        private void Start()
        {
            MonoGenericPool<EnemySpawnParticle>.Initialize(enemySpawnParticle);

            StartCoroutine("EnemyWavesCoroutin");

            Invoke("TimesOut", endTimeSecond);
        }

        private void TimesOut()
        {
            Debug.Log("¾ß±â¿î~~!!!!!!!!!");

            isGameEnd = true;
            StopAllCoroutines();

            sceneManagerSO.LevelClear();
            foreach(var enemy in spawnEnemyList)
            {
                enemy.transform.GetComponent<BaseEnemyHealth>().TakeDamage(actionData);
            }
        }

        private IEnumerator EnemyWavesCoroutin()
        {
            for(int i = 0; i < waveCount; i++)
            {
                int randomWave = Random.Range(0, spawnEnemies.Count);
                Debug.Log(randomWave);

                for(int j = 0; j < spawnPosition.Length; j++)
                {
                    var nowEnemy = Instantiate
                    (
                    spawnEnemies[randomWave].spawnInfos[j].enemy,
                    spawnPosition[j].position,
                    Quaternion.identity
                    );

                    EnemySpawnParticle spawnParticle = MonoGenericPool<EnemySpawnParticle>.Pop();
                    spawnParticle.transform.position = spawnPosition[j].position;


                    spawnEnemyList.Add( nowEnemy );

                    yield return new WaitForSeconds(1);
                }

                yield return new WaitForSeconds(waveCooltime);
            }
        }
    }
}
