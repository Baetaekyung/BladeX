using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BaseStateEntityAnimation<StateEnum, Entity> : State<StateEnum>
        where StateEnum : Enum
        where Entity : global::Entity
    {
        protected readonly AnimationParameterSO baseAnimParam;
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

        protected virtual void OnAnimationEndTrigger()                      => Debug.Log("OnAnimationEnd");
        protected virtual void OnAnimationEndableTrigger()                  => Debug.Log("OnanimatoinEndable");
        protected virtual void OnAnimationEndTriggerListen()                => Debug.Log("OnAnimationListen");
        protected virtual void OnAnimationEndTriggerStoplisten()            => Debug.Log("Onstoplisten");

        protected virtual void OnAttackTrigger()                            => Debug.Log("OnAttackTrigger");
        protected virtual void OnAllowRotateAllowTrigger()                  => Debug.Log("OnRotoateSet");
        protected virtual void OnAllowRotateDisallowTrigger()               => Debug.Log("OnRotoateDisallowSet");

        protected virtual void OnForceEventTrigger(float force)             => Debug.Log("OnFroce");
        protected virtual void OnSpeedMultiplierDefaultTrigger(float set)   => Debug.Log("OnSpeedmultiplier");
        //protected virtual void OnMovementSetTrigger(Vector3 value) => Debug.Log("Onmovementset");
        public override void Enter()
        {
            base.Enter();

            animationTriggers.OnAnimationEndEvent += OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndableEvent += OnAnimationEndableTrigger;
            animationTriggers.OnAnimationEndTriggeristenEvent += OnAnimationEndTriggerListen;
            animationTriggers.OnAnimationEndTriggerStopListenEvent += OnAnimationEndTriggerStoplisten;

            animationTriggers.OnAttackTriggerEvent += OnAttackTrigger;
            animationTriggers.OnRotateAllowSetEvent += OnAllowRotateAllowTrigger;
            animationTriggers.OnRotateDisallowSetEvent+= OnAllowRotateDisallowTrigger;

            animationTriggers.OnForceEvent += OnForceEventTrigger;
            animationTriggers.OnSpeedMultiplierDefaultEvent += OnSpeedMultiplierDefaultTrigger;
            //animationTriggers.OnMovementSetEvent += OnMovementSetTrigger;
            if (baseAnimParam != null)
                PlayAnimationOnEnter();
        }


        public override void Exit()
        {
            animationTriggers.OnAnimationEndEvent -= OnAnimationEndTrigger;
            animationTriggers.OnAnimationEndableEvent -= OnAnimationEndableTrigger;
            animationTriggers.OnAnimationEndTriggeristenEvent -= OnAnimationEndTriggerListen;
            animationTriggers.OnAnimationEndTriggerStopListenEvent -= OnAnimationEndTriggerStoplisten;

            animationTriggers.OnAttackTriggerEvent -= OnAttackTrigger;
            animationTriggers.OnRotateAllowSetEvent -= OnAllowRotateAllowTrigger;
            animationTriggers.OnRotateDisallowSetEvent -= OnAllowRotateDisallowTrigger;

            animationTriggers.OnForceEvent -= OnForceEventTrigger;
            animationTriggers.OnSpeedMultiplierDefaultEvent -= OnSpeedMultiplierDefaultTrigger;
            //animationTriggers.OnMovementSetEvent -= OnMovementSetTrigger;
            base.Exit();
            //re init
            OnAllowRotateAllowTrigger();
            OnSpeedMultiplierDefaultTrigger(1);
        }

        protected virtual void PlayAnimationOnEnter()
        {
            PlayAnimationRebind(baseAnimParam.GetAnimationHash, -1);
            //ownerAnimator.Play(baseAnimParam.GetAnimationHash, -1);
        }
        protected void PlayAnimation(AnimationParameterSO param, int layer = -1)
        {
            Debug.Assert(param != null, "parameterSO is null");
            ownerAnimator.Play(param.GetAnimationHash, layer);
        }

        protected void PlayAnimation(int hash, float normalizedTime = 0)
        {
            ownerAnimator.Play(hash, -1, normalizedTime);
        }
        protected void PlayAnimationRebind(AnimationParameterSO param, int layer = -1)
        {
            Debug.Assert(param != null, "parameterSO is null");
            int hash = param.GetAnimationHash;
            ownerAnimator.Rebind();
            ownerAnimator.Play(param.GetAnimationHash, layer);
            //ownerAnimator.CrossFadeInFixedTime(hash, 0.05f, layer);
        }
        protected void PlayAnimationRebind(int hash, int layer = -1)
        {
            Debug.Log("dd");
            ownerAnimator.Rebind();
            ownerAnimator.Play(hash, layer);
            //ownerAnimator.CrossFadeInFixedTime(hash, 0.05f, layer, 0, 0.2f);
        }
    }
}
