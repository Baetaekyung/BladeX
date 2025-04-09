using UnityEngine;
using System.Collections.Generic;
using Swift_Blade.Enemy.Goblin;

namespace Swift_Blade.Enemy.Boss.Goblin
{
    public class GoblinBoss : BaseGoblin
    {
        [Space] 
        [Header("Summon info")] 
        [SerializeField] private GoblinEnemyInBoss summonPrefab;
        
        [SerializeField] private int maxSummonCount;
        [SerializeField] private int minSummonCount;
        [SerializeField] private float summonRadius;
        private List<GoblinEnemyInBoss> summons;

        private GoblinBossVFXPlayer goblinBossVFXPlayer;
        
        protected override void Start()
        {
            base.Start();
            
            player = target.GetComponent<Player>().GetPlayerTransform;
            summons = new List<GoblinEnemyInBoss>();
            goblinBossVFXPlayer = GetComponent<GoblinBossVFXPlayer>();
        }

        public void Summon()
        {
            var rand = Random.Range(minSummonCount, maxSummonCount);

            for (var i = 0; i < rand; i++)
            {
                var randomPos = Random.insideUnitCircle * summonRadius;
                var spawnPosition = new Vector3(transform.position.x + randomPos.x, transform.position.y,
                    transform.position.z + randomPos.y);

                var newGoblin = Instantiate(summonPrefab, spawnPosition, Quaternion.identity);
                newGoblin.Init(this);

                goblinBossVFXPlayer.PlaySpawnEffect(spawnPosition);
                
                summons.Add(newGoblin);
            }
        }

        public bool CanCreateSummon()
        {
            return summons.Count <= 0;
        }
        
        public void RemoveInSummonList(GoblinEnemyInBoss _goblin)
        {
            summons.Remove(_goblin);
            if (summons.Count == 0)
                summons.Clear();
        }
                
    }
}