using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerState : BaseStateEntityAnimation<PlayerStateEnum, Player>
    {
        protected readonly Player player;

        protected BasePlayerState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            player = entity;
        }
        protected virtual void OnAttackInput(EPlayerAttackPreviousState currentState)
        {
            player.Attack(currentState);
        }
        protected virtual void OnParryInput()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Parry);
        }
        protected virtual void OnDashInput()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Dash);
        }
    }
}
