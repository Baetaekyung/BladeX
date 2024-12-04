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

        protected virtual void OnAnimationEndTrigger()
        {
            Debug.Log("onAnimationEnd");
        }
        protected virtual void OnAnimationEndTriggerListen()
        {
            Debug.Log("OnAnimationListen");
        }
        protected virtual void OnAnimationEndableTrigger()
        {
            Debug.Log("onanimatoinEndable");
        }
        protected virtual void OnForceEventTrigger(float force)
        {
            Debug.Log("onfroce");
        }
        protected virtual void OnMovementSet(float set)
        {
            Debug.Log("onmovementset");
        }
        public override void Enter()
        {
            base.Enter();
            animationTriggers.OnAnimationEnd += OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndableListen += OnAnimationEndTriggerListen;
            animationTriggers.OnAnimationEndable += OnAnimationEndableTrigger;
            animationTriggers.OnForceEvent += OnForceEventTrigger;
            animationTriggers.OnMovementSetEvent += OnMovementSet;
            PlayAnimationOnEnter();
        }
        public override void Exit()
        {
            animationTriggers.OnAnimationEnd -= OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndableListen -= OnAnimationEndTriggerListen;
            animationTriggers.OnAnimationEndable -= OnAnimationEndableTrigger;
            animationTriggers.OnForceEvent -= OnForceEventTrigger;
            animationTriggers.OnMovementSetEvent -= OnMovementSet;
            base.Exit();
        }
        public virtual void PlayAnimationOnEnter()
        {
            if(baseAnimParam != null)
                ownerAnimator.Play(baseAnimParam.GetAnimationHash, -1, baseAnimParam.GetNormalizedTime);
        }
        protected void PlayAnimation(AnimationParameterSO param)
        {
            ownerAnimator.Play(param.GetAnimationHash, -1, param.GetNormalizedTime);
        }

        protected void PlayAnimation(int hash, float normalizedTime = 0)
        {
            ownerAnimator.Play(hash, -1, normalizedTime);
        }
        public virtual void StopAnimation()
        {
        }
    }
}
