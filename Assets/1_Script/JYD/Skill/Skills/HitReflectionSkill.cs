using System.Linq;
using Swift_Blade.Combat.Health;
using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Skill
{
    [CreateAssetMenu(fileName = "HitReflectionSkill", menuName = "SO/Skill/Hit/Reflection")]
    public class HitReflectionSkill : SkillData
    {
        [SerializeField][Range(1,100)] private float random;
        [SerializeField] private float knockbackForce;
        [SerializeField] private LayerMask whatIsTarget;
        private float skillRadius = 10;
        public override void Initialize()
        {
            MonoGenericPool<AttackReflectionParticle>.Initialize(skillParticle);
        }

        public override void UseSkill(Player player, Transform[] targets = null)
        {
            if (targets == null)
            {
                targets = Physics.OverlapSphere(player.GetPlayerTransform.position, skillRadius, whatIsTarget)
                    .Select(x => x.transform).ToArray();
            }
    
            AttackReflectionParticle attackReflectionParticle = MonoGenericPool<AttackReflectionParticle>.Pop();
            attackReflectionParticle.transform.position = player.GetPlayerTransform.position + new Vector3(0, 0.5f, 0);

            Transform closeTarget = null;
            float minDistance = float.MaxValue;
            foreach (var item in targets)
            {
                float distance = Vector3.Distance(item.position, player.GetPlayerTransform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closeTarget = item;
                }
            }
            
            float randomValue = Random.Range(0f, 100f); 
            if (randomValue < random)
            {
                if (closeTarget != null && closeTarget.TryGetComponent(out BaseEnemyHealth health))
                {
                    ActionData actionData = new ActionData
                    {
                        damageAmount = 1,
                        knockbackForce = knockbackForce,
                        knockbackDirection = (closeTarget.position - player.GetPlayerTransform.position).normalized,
                        stun = true
                    };
                    health.TakeDamage(actionData);
                    
                }
            }
        }

    }
}
