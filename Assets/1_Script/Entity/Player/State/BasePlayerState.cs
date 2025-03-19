using Swift_Blade.Audio;
using System;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerState : BaseStateEntityAnimation<PlayerStateEnum, Player>
    {
        protected readonly Player player;

        protected readonly PlayerMovement playerMovement;
        protected readonly PlayerInput playerInput;

        //private static float additionalZValue;
        protected static Vector3 anim_inputLocalLerp;
        protected virtual bool BaseAllowAttackInput { get; } = true;
        protected virtual bool BaseAllowParryInput { get; } = true;
        protected virtual bool BaseAllowDashInput { get; } = true;

        private const float delayParry = 1;
        private const float delayDash = 1.5f;

        private static float nextDelayTime_AllowParry;
        private static float nextDelayTime_AllowDash;

        protected BasePlayerState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            player = entity;
            playerMovement = player.GetPlayerMovement;
            playerInput = player.GetPlayerInput;
        }
        public override void Enter()
        {
            base.Enter();
            //additionalZValue = 0;
            playerMovement.UseMouseLock = false;
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && BaseAllowAttackInput)
                OnAttackInput(EComboState.LightAttack);
            if (Input.GetKeyDown(KeyCode.Mouse1) && BaseAllowAttackInput)
                OnAttackInput(EComboState.PowerAttack);

            if (Input.GetKeyDown(KeyCode.C) && BaseAllowParryInput)
                OnParryInput();
            if (Input.GetKeyDown(KeyCode.Space) && BaseAllowDashInput && playerInput.GetInputDirectionRaw.sqrMagnitude > 0.25f && playerMovement.CanRoll)
                OnDashInput();

            //if (Input.GetKeyDown(KeyCode.N))
            //    GetOwnerFsm.ChangeState(PlayerStateEnum.Dead);

            //movement
            Quaternion CameraRotation = playerInput.CameraRotationOnlyY;
            Vector3 localInput = playerInput.GetInputDirectionRaw;
            Vector3 resultVector = CameraRotation * localInput;
            player.GetPlayerMovement.InputDirection = resultVector;

            //animator
            Vector3 resultAnimatorInput = resultVector;
            anim_inputLocalLerp = Vector3.MoveTowards(anim_inputLocalLerp, resultAnimatorInput, Time.deltaTime * 8);
            Transform playerTransform = player.GetPlayerRenderer.GetPlayerVisualTrasnform;
            Vector3 anim_inputLocal = playerTransform.InverseTransformDirection(anim_inputLocalLerp);
            Debug.DrawRay(Vector3.zero, anim_inputLocal, Color.red, 0.1f);
            player.GetPlayerAnimator.GetAnimator.SetFloat("X", anim_inputLocal.x);

            player.GetPlayerAnimator.GetAnimator.SetFloat("Z", anim_inputLocal.z);

            UI_DebugPlayer.DebugText(7, anim_inputLocalLerp, "animLocalLerp");
        }
        /// <summary>
        /// </summary>
        /// <param name="previousComboState">current Combo State</param>
        protected virtual void OnAttackInput(EComboState previousComboState, EComboState nonImeediateState = EComboState.None)
        {
            player.Attack(previousComboState, nonImeediateState);
        }
        protected virtual void OnParryInput()
        {
            if (nextDelayTime_AllowParry > Time.time) return;
            nextDelayTime_AllowParry = Time.time + delayParry;
            GetOwnerFsm.ChangeState(PlayerStateEnum.Parry);
        }
        protected virtual void OnDashInput()
        {
            if (nextDelayTime_AllowDash > Time.time) return;
            nextDelayTime_AllowDash = Time.time + delayDash;
            GetOwnerFsm.ChangeState(PlayerStateEnum.Roll);
        }
        protected sealed override void OnAllowRotateAllowTrigger() => playerMovement.AllowRotate = true;
        protected sealed override void OnAllowRotateDisallowTrigger() => playerMovement.AllowRotate = false;
        protected override void OnAnimationEndTrigger()
        {
            GetOwnerFsm.ChangeState(PlayerStateEnum.Move);
        }
        protected sealed override void OnSpeedMultiplierDefaultTrigger(float set) => playerMovement.SpeedMultiplierDefault = set;
        protected override void OnAudioPlayTrigger(AudioSO audioSO)
        {
            AudioManager.PlayWithInit(audioSO, true);
        }
        //protected sealed override void OnMovementSetTrigger(Vector3 value) => playerMovement.SetAdditionalVelocity(value);
        protected sealed override void OnAttackTrigger()
        {
            if (player.GetPlayerDamageCaster.Cast())
            {
                //player.GetEntityComponent<PlayerStatCompo>().GetStyleMeter.SuccessHit();
            }
        }
    }
}
