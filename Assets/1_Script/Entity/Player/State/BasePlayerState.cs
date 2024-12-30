using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerState : BaseStateEntityAnimation<PlayerStateEnum, Player>
    {
        protected BasePlayerState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
        protected override void OnAnimationEndableTrigger() => GetOwnerFsm.ChangeState(PlayerStateEnum.Movement);
    }
}
