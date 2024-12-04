using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerParryState : BasePlayerState
    {
        public PlayerParryState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
        }
    }
}
