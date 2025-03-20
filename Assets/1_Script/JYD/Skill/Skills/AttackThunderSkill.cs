using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;
using System;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "AttackThunderSkill", menuName = "SO/Skill/Attack/Thunder")]
    public class AttackThunderSkill : SkillData
    {
        [SerializeField] private ushort attackCount = 3;
        private float attackCounter = 0;
        
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private float skillRadius;
        [SerializeField] private int skillDamage;

        public override void Initialize()
        {
            if (SkillEffectPrefab == null || SkillEffectPrefab.GetMono == null)
            {
                Debug.LogError("SkillEffectPrefab or its MonoBehaviour is null.");
                return;
            }

            attackCounter = 0;
            MonoGenericPool<ThunderParticle>.Initialize(SkillEffectPrefab);
        }

        public override void UseSkill(Transform transform)
        {
            ++attackCounter;
            Debug.Log(attackCounter);
            if (attackCounter >= attackCount)
            {
                Collider[] targets = Physics.OverlapSphere(transform.position , skillRadius , whatIsTarget);
                foreach (var item in targets)
                {
                    if(item.TryGetComponent(out BaseEnemyHealth health))
                    {
                        ActionData actionData= new ActionData();
                        actionData.damageAmount = skillDamage;
                        
                        health.ChangeParryState();
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
