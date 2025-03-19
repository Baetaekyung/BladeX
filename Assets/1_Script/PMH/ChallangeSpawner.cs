using System.Collections;
using System.Collections.Generic;
using Swift_Blade.Combat.Health;
using Swift_Blade.Enemy;
using UnityEngine;

namespace Swift_Blade
{
    public class ChallangeSpawner : MonoBehaviour
    {
        private ActionData actionData;
        [SerializeField] private SceneManagerSO sceneManagerSO;
        public List<SpawnInfos> spawnEnemies;

        public List<BaseEnemy> spawnEnemyList;

        [SerializeField] private int waveCount;
        [SerializeField] private float waveCooltime;

        [SerializeField] private float endTimeSecond;

        [SerializeField] private Transform[] spawnPosition;

        private void Awake()
        {
            actionData.damageAmount = 99;
            actionData.dealer = default;
            actionData.hitNormal = default;
        }
        private void Start()
        {
            StartCoroutine("EnemyWavesCoroutin");

            Invoke("TimesOut", endTimeSecond);
        }

        private void TimesOut()
        {
            Debug.Log("¾ß±â¿î~~!!!!!!!!!");
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
                yield return new WaitForSeconds(waveCooltime);

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

                    spawnEnemyList.Add( nowEnemy );
                }
            }
        }
    }
}
