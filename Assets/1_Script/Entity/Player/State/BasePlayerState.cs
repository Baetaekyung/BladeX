using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerState : BaseStateEntityAnimation<PlayerStateEnum, Player>
    {
        protected BasePlayerState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        public override void Enter()
        {
            base.Enter();
            PlayerInput.OnParryInput += HandleOnParry;
        }
        public override void Exit()
        {
            PlayerInput.OnParryInput -= HandleOnParry;
            base.Exit();
        }
        protected virtual void HandleOnParry()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Parry);
        }
    }
}
