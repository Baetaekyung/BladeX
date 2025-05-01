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
        [SerializeField] private  float disableTime = 3f;
        [SerializeField] private  float radius;
        [SerializeField] private  float defaultSlowValue;
        [SerializeField] private  float minSlowValue;
        [SerializeField] private  LayerMask whatIsEnemy;
        
        
        private float disableTimer;
        private bool useSkill = false;
        
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
            disableTimer = 0;
            useSkill = true;
            
            PlayParticle(player);
        }
        
        private void PlayParticle(Player player)
        {
            if (areaTyphoonParticle == null)
            {
                areaTyphoonParticle = MonoGenericPool<AreaTyphoonParticle>.Pop();
                areaTyphoonParticle.SetFollowTransform(player.GetPlayerTransform);
            }
                        
        }
        
        public override void SkillUpdate(Player player, IEnumerable<Transform> targets = null)
        {
            if (!useSkill || !CheckDisableTime()) return;
            
            PlayParticle(player);
            
            currentEnemies.Clear();
            enemiesToRemove.Clear();
            
            targets = Physics.OverlapSphere(player.GetPlayerTransform.position, radius, whatIsEnemy)
                            .Select(c => c.transform);
            
            foreach (var item in targets)
            {
                if (item.TryGetComponent(out BaseEnemy enemy))
                {
                    currentEnemies.Add(enemy);
                    
                    if (allEnemies.Add(enemy))
                    {
                        float animationSpeed = Mathf.Max(minSlowValue, defaultSlowValue);
                        enemy.SetSlowMotionSpeed(animationSpeed);
                    }
                    
                }
            }
            
            
            foreach (var enemy in allEnemies)
            {
                if (!currentEnemies.Contains(enemy))
                {
                    enemiesToRemove.Add(enemy);
                    enemy.ResetSlowMotionSpeed();
                }
            }
            
            foreach (var enemy in enemiesToRemove)
            {
                allEnemies.Remove(enemy);
            }
        }
        
        private bool CheckDisableTime()
        {
            disableTimer += Time.deltaTime;
            if (disableTimer >= disableTime)
            {
                ResetSkill();
                return false;
            }
            
            return true;
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
                areaTyphoonParticle.PushSelf();
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