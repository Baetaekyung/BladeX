using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerHitStunState : BasePlayerState
    {
        protected override bool BaseAllowAttackInput => false;
        protected override bool BaseAllowDashInput => false;
        protected override bool BaseAllowSpecialInput => false;
        private readonly PlayerHealth playerHealth;
        public PlayerHitStunState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            playerHealth = entity.GetPlayerHealth;
        }
        public override void Enter()
        {
            base.Enter();
            playerMovement.AllowInputMove = false;
            playerMovement.AllowRotate = false;
            playerHealth.IsPlayerInvincible = true;
        }
        public override void Exit()
        {
            playerMovement.AllowInputMove = true;
            playerMovement.AllowRotate = true;
            playerHealth.IsPlayerInvincible = false;
            base.Exit();
        }
    }
}
