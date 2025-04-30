using System.Collections.Generic;
using Swift_Blade.Enemy;
using Swift_Blade.Pool;
using UnityEngine;
using System.Linq;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "SlowAreaSkill",menuName = "SO/Skill/Cyan/SlowAreaSkill")]
    public class SlowAreaSkill : SkillData
    {
        public float disableTime = 3f;
        public float radius;
        public float minSlowValue;
        public LayerMask whatIsEnemy;
        
        private float disableTimer;
        private bool useSkill;
        
        private AreaTyphoonParticle areaTyphoonParticle;
        
        private readonly HashSet<BaseEnemy> allEnemies = new HashSet<BaseEnemy>();
        private readonly HashSet<BaseEnemy> currentEnemies = new HashSet<BaseEnemy>();
        private readonly List<BaseEnemy> enemiesToRemove = new List<BaseEnemy>();
        
        public override void Initialize()
        {
            allEnemies.Clear();
            currentEnemies.Clear();
            enemiesToRemove.Clear();
            
            MonoGenericPool<AreaTyphoonParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, IEnumerable<Transform> targets = null)
        {
            useSkill = true;
            disableTimer = 0;
            
            if (areaTyphoonParticle == null)
            {
                areaTyphoonParticle = MonoGenericPool<AreaTyphoonParticle>.Pop();
                areaTyphoonParticle.SetFollowTransform(player.GetPlayerTransform);
            }
        }
        
        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (useSkill == false) return;
            
            disableTimer += Time.deltaTime;
                
            if (disableTimer >= disableTime)
            {
                ResetSkill();
                return;
            }
            
            areaTyphoonParticle.SetFollowTransform(player.GetPlayerTransform);
                
            targets = Physics.OverlapSphere(player.GetPlayerTransform.position, radius, whatIsEnemy)
                            .Select(c => c.transform);
            
            currentEnemies.Clear();
            
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemy enemy))
                {
                    currentEnemies.Add(enemy);
                    
                    if (allEnemies.Add(enemy))
                    {
                        float animationSpeed = Mathf.Max(minSlowValue, 0.6f);
                        enemy.SetSlowMotionSpeed(animationSpeed);
                    }
                    
                }
            }
            
            enemiesToRemove.Clear();
            
            foreach (var enemy in allEnemies)
            {
                if (!currentEnemies.Contains(enemy))
                {
                    enemiesToRemove.Add(enemy);
                    Debug.Log("Reset : ");
                    enemy.ResetSlowMotionSpeed();
                }
            }
            
            foreach (var enemy in enemiesToRemove)
            {
                allEnemies.Remove(enemy);
            }
        }
        
        public override void ResetSkill()
        {
            useSkill = false;
            disableTimer = 0;
                        
            foreach (var enemy in allEnemies)
            {
                enemy.ResetSlowMotionSpeed();
            }
            
            allEnemies.Clear();
            currentEnemies.Clear();
            enemiesToRemove.Clear();
            
            if (areaTyphoonParticle != null)
            {
                MonoGenericPool<AreaTyphoonParticle>.Push(areaTyphoonParticle);
                areaTyphoonParticle = null;
            }
        }
        
        public override void DrawGizmo(Player player)
        {
            if (player == null)return;
                
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.transform.position, radius);
        }
        
    }
}