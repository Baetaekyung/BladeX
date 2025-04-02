using System;
using Unity.AppUI.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerAnimator : MonoBehaviour, IEntityComponent
    {
        private PlayerStatCompo playerStatCompo;
        
        [SerializeField] private Animator animator;
        public Animator GetAnimator => animator;

        private int attackSpeedHash = Animator.StringToHash("AttackSpeed");
        private float attackAnimationSpeed = 1;
        
        public void EntityComponentAwake(Entity entity)
        {
            playerStatCompo = (entity as Player).GetPlayerStat;
            var attackSpeedStat = playerStatCompo.GetStat(StatType.ATTACKSPEED);
            
            attackSpeedStat.OnValueChanged += SetPlayerAttackSpeed;
            attackAnimationSpeed = attackSpeedStat.Value;
            
        }

        private void OnDestroy()
        {
            playerStatCompo.GetStat(StatType.ATTACKSPEED).OnValueChanged -= SetPlayerAttackSpeed;
        }
        
        public void PlayAnimation(int hash, int layer = -1)
        {
            animator.Play(hash, layer);
        }
        
        private void SetPlayerAttackSpeed()
        {
            float speed = playerStatCompo.GetStat(StatType.ATTACKSPEED).Value;
            attackAnimationSpeed = speed;
            animator.SetFloat(attackSpeedHash, speed);
            
        }

        private void LateUpdate()
        {
            if (animator.GetFloat(attackSpeedHash) != attackAnimationSpeed)
            {
                animator.SetFloat(attackSpeedHash, attackAnimationSpeed);
            }
                        
            
        }
    }
}
