using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Boss.Goblin
{
    public class GoblinBoss : BaseBoss
    {
        public float stopDistance;
        
        [Space]
        [Header("Summon info")]
        public GoblinEnemy summonPrefab;
        public int maxSummonCount;
        public int minSummonCount;

        public float summonRadius;
        
        private List<GoblinEnemy> summons;
        
        protected override void Start()
        {
            base.Start();
            summons = new List<GoblinEnemy>();
        }

        protected override void Update()
        {
            if (bossAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (bossAnimationController.isManualMove)
            {
                float distance = Vector3.Distance(transform.position , target.position);
                
                if (distance > stopDistance)
                {
                    attackDestination = transform.position + transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                        bossAnimationController.AttackMoveSpeed * Time.deltaTime);
                }
            }

            if (bossAnimationController is GoblinAnimatorController goblinAnimatorController)
            {
                if (goblinAnimatorController.isManualKnockback)
                {
                    attackDestination = transform.position + -transform.forward;

                    transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                        goblinAnimatorController.knockbackSpeed * Time.deltaTime); 
                }
            }
        }
        
        
        public void Summon()
        {
            int rand = Random.Range(minSummonCount , maxSummonCount);
            
            for (int i = 0; i < rand; i++)
            {
                Vector2 randomPos = Random.insideUnitCircle * summonRadius;
                Vector3 spawnPosition = new Vector3(transform.position.x + randomPos.x, transform.position.y, transform.position.z + randomPos.y);

                GoblinEnemy newGoblin = Instantiate(summonPrefab, spawnPosition, Quaternion.identity);
                newGoblin.Init(this);
                
                summons.Add(newGoblin);
            }
            
        }

        public bool CanCreateSummon() => summons.Count <= 0 ? true : false;
        
        public void RemoveInSummonList(GoblinEnemy _goblin)
        {
            summons.Remove(_goblin);
            if(summons.Count == 0)
                summons.Clear();
        }
        
        
    }
}
