using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerHitStunState : BasePlayerState
    {
        protected override bool BaseAllowAttackInput => false;
        protected override bool BaseAllowDashInput => false;
        protected override bool BaseAllowParryInput => false;
        public PlayerHitStunState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        public override void Enter()
        {
            base.Enter();
            playerMovement.AllowInputMove = false;
            playerMovement.AllowRotate = false;
        }
        public override void Exit()
        {
            playerMovement.AllowInputMove = true;
            playerMovement.AllowRotate = true;

            base.Exit();
        }
    }
}
