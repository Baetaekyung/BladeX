using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;
using System;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "AttackThunderSkill", menuName = "SO/Skill/Attack/Thunder")]
    public class AttackThunderSkill : SkillData
    {
        [Tooltip("몇번 때리면 기절할게 할것인지.")] 
        [SerializeField] private ushort attackCount = 3;
        private float attackCounter = 0;
       
        [SerializeField] private int skillDamage = 0;

        public override void Initialize()
        {
            if (skillParticle == null || skillParticle.GetMono == null)
            {
                Debug.LogError("SkillEffectPrefab or its MonoBehaviour is null.");
                return;
            }

            attackCounter = 0;
            MonoGenericPool<ThunderParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player,Transform[] targets = null)
        {
            ++attackCounter;
            //Debug.Log(attackCounter);
            if (attackCounter >= attackCount)
            {
                foreach (var item in targets)
                {
                    if(item.TryGetComponent(out BaseEnemyHealth health))
                    {
                        ActionData actionData= new ActionData();
                        actionData.damageAmount = skillDamage;
                        actionData.stun = true;
                        
                        health.TakeDamage(actionData);
                        
                        ThunderParticle th = MonoGenericPool<ThunderParticle>.Pop();
                        th.transform.position = item.transform.position + new Vector3(0,1,0);
                    }
                }

                attackCounter = 0;
            }
            
        }
        
    }
}
