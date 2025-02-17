using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerDeadState : BasePlayerState
    {
        protected override bool BaseAllowAttackInput => false;
        protected override bool BaseAllowDashInput => false;
        protected override bool BaseAllowParryInput => false;
        public event Action OnPlayerDead;
        public PlayerDeadState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        public override void Enter()
        {
            base.Enter();
            OnPlayerDead?.Invoke();
            playerMovement.AllowRotate = false;
            playerMovement.AllowInputMove = false;
        }
    }
}
