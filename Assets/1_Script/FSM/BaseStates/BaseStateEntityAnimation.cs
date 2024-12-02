using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BaseStateEntityAnimation<StateEnum, Entity> : State<StateEnum>
        where StateEnum : Enum
    {
        private readonly AnimationParameterSO baseAnimParam;
        private readonly Animator ownerAnimator;
        private readonly AnimationTriggers animationTriggers;
        protected readonly Entity entity;
        protected BaseStateEntityAnimation(FiniteStateMachine<StateEnum> stateMachine, Animator animator, Entity entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine)
        {
            baseAnimParam = animParamSO;
            ownerAnimator = animator;
            animationTriggers = animTrigger;
            this.entity = entity;
        }

        public virtual void OnAnimationEndTrigger()
        {
            Debug.Log("onAnimationEnd");
        }
        public virtual void OnAnimationEndableTrigger()
        {
            Debug.Log("onanimatoinEndable");
        }
        public override void Enter()
        {
            base.Enter();
            animationTriggers.OnAnimationEnd += OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndable += OnAnimationEndableTrigger;
            PlayAnimation();
        }
        public override void Exit()
        {
            animationTriggers.OnAnimationEnd -= OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndable -= OnAnimationEndableTrigger;
            base.Exit();
        }
        public virtual void PlayAnimation()
        {
            ownerAnimator.Play(baseAnimParam.GetAnimationHash, -1, 0);
        }
        public virtual void StopAnimation()
        {
        }
    }
}
