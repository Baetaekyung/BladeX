using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Boss.Goblin
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
        
        protected override void Start()
        {
            base.Start();
            summons = new List<GoblinEnemyInBoss>();
        }
        
        public void Summon()
        {
            int rand = Random.Range(minSummonCount , maxSummonCount);
            
            for (int i = 0; i < rand; i++)
            {
                Vector2 randomPos = Random.insideUnitCircle * summonRadius;
                Vector3 spawnPosition = new Vector3(transform.position.x + randomPos.x, transform.position.y, transform.position.z + randomPos.y);

                GoblinEnemyInBoss newGoblin = Instantiate(summonPrefab, spawnPosition, Quaternion.identity);
                newGoblin.Init(this);
                
                summons.Add(newGoblin);
            }
            
        }
        public bool CanCreateSummon() => summons.Count <= 0;
        
        public void RemoveInSummonList(GoblinEnemyInBoss _goblin)
        {
            summons.Remove(_goblin);
            if(summons.Count == 0)
                summons.Clear();
        }

        public override void SetDead()
        {
            StopImmediately();
            goblinAnimator.StopAllAnimationEvents();
        }
    }
}
