using System;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerAnimator : MonoBehaviour, IEntityComponent
    {
        private PlayerStatCompo playerStatCompo;
        
        [SerializeField] private Animator animator;
        public Animator GetAnimator => animator;

        private int attackSpeedHash = Animator.StringToHash("AttackSpeed");
        
        public void EntityComponentAwake(Entity entity)
        {
            playerStatCompo = (entity as Player).GetPlayerStat;
            playerStatCompo.GetStat(StatType.ATTACKSPEED).OnValueChanged += SetPlayerAttackSpeed;
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
            animator.SetFloat(attackSpeedHash, speed);
        }
    }
}
