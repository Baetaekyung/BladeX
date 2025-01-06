using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BaseStateEntityAnimation<StateEnum, Entity> : State<StateEnum>
        where StateEnum : Enum
        where Entity : global::Entity
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
        protected virtual void OnAttackTrigger()
        {
            Debug.Log("OnAttackTrigger");
        }
        protected virtual void OnForceEventTrigger(float force)
        {
            Debug.Log("onfroce");
        }
        protected virtual void OnSpeedMultiplierDefaultTrigger(float set)
        {
            Debug.Log("onspeedmultiplier");
        }
        protected virtual void OnMovementSetTrigger(Vector3 value)
        {
            Debug.Log("onmovementset");
        }
        public override void Enter()
        {
            base.Enter();
            animationTriggers.OnAnimationEndEvent += OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndableListenEvent += OnAnimationEndTriggerListen;
            animationTriggers.OnAnimationnEndableEvent += OnAnimationEndableTrigger;

            animationTriggers.OnAttackTriggerEvent += OnAttackTrigger;

            animationTriggers.OnForceEvent += OnForceEventTrigger;
            animationTriggers.OnSpeedMultiplierDefaultEvent += OnSpeedMultiplierDefaultTrigger;
            //animationTriggers.OnMovementSetEvent += OnMovementSetTrigger;
            PlayAnimationOnEnter();
        }


        public override void Exit()
        {
            animationTriggers.OnAnimationEndEvent -= OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndableListenEvent -= OnAnimationEndTriggerListen;
            animationTriggers.OnAnimationnEndableEvent -= OnAnimationEndableTrigger;

            animationTriggers.OnAttackTriggerEvent -= OnAttackTrigger;

            animationTriggers.OnForceEvent -= OnForceEventTrigger;
            animationTriggers.OnSpeedMultiplierDefaultEvent -= OnSpeedMultiplierDefaultTrigger;
            //animationTriggers.OnMovementSetEvent -= OnMovementSetTrigger;
            base.Exit();
        }

        public virtual void PlayAnimationOnEnter()
        {
            if (baseAnimParam != null)
            {
                Debug.Log(baseAnimParam);
                ownerAnimator.Play(baseAnimParam.GetAnimationHash, -1);
            }
        }
        protected void PlayAnimation(AnimationParameterSO param)
        {
            Debug.Assert(param != null, "parameterSO is null");
            ownerAnimator.Play(param.GetAnimationHash, -1);
        }

        protected void PlayAnimation(int hash, float normalizedTime = 0)
        {
            ownerAnimator.Play(hash, -1, normalizedTime);
        }
    }
}
