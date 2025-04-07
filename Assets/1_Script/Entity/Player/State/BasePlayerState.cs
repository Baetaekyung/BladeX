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

        protected static Vector3 anim_inputLocalLerp;
        protected virtual bool BaseAllowAttackInput { get; } = true;
        protected virtual bool BaseAllowSpecialInput { get; } = true;
        protected virtual bool BaseAllowDashInput { get; } = true;
        protected Vector3 GetResultVector => playerInput.CameraRotationOnlyY * playerInput.GetInputDirectionRaw;

        protected float GetSpecialDelay => PlayerWeaponManager.CurrentWeapon.GetSpecialDelay;
        protected float GetRollDelay => PlayerWeaponManager.CurrentWeapon.GetRollDelay;

        private static float nextDelayTime_AllowSpecial;
        private static float nextDelayTime_AllowRoll;

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
            if (PopupManager.Instance.IsRemainPopup) return;

            if (Input.GetKeyDown(KeyCode.Mouse0) && BaseAllowAttackInput)
                OnAttackInput(EComboState.LightAttack);
            if (Input.GetKeyDown(KeyCode.Mouse1) && BaseAllowAttackInput)
                OnAttackInput(EComboState.PowerAttack);

            if (Input.GetKeyDown(KeyCode.C) && BaseAllowSpecialInput)
                OnSpecialInput();

            if (Input.GetKeyDown(KeyCode.Space) && BaseAllowDashInput && playerInput.GetInputDirectionRaw.sqrMagnitude > 0.25f && playerMovement.CanRoll)
                OnDashInput();

            //movement
            Quaternion CameraRotation = playerInput.CameraRotationOnlyY;
            Vector3 localInput = playerInput.GetInputDirectionRaw;
            Vector3 resultVector = CameraRotation * localInput;

            UI_DebugPlayer.DebugText(0, resultVector, "res", DBG_UI_KEYS.Keys_PlayerMovement);
            OnApplyMovement(resultVector);

            //animator
            Vector3 resultAnimatorInput = resultVector;
            anim_inputLocalLerp = Vector3.MoveTowards(anim_inputLocalLerp, resultAnimatorInput, Time.deltaTime * 8);
            Transform playerTransform = player.GetPlayerRenderer.GetPlayerVisualTrasnform;
            Vector3 anim_inputLocal = playerTransform.InverseTransformDirection(anim_inputLocalLerp);
            //Debug.DrawRay(Vector3.zero, anim_inputLocal, Color.red, 0.1f);

            player.GetPlayerAnimator.GetAnimator.SetFloat("X", anim_inputLocal.x);
            player.GetPlayerAnimator.GetAnimator.SetFloat("Z", anim_inputLocal.z);
        }
        /// <summary>
        /// </summary>
        /// <param name="resultVector">not normalized</param>
        protected virtual void OnApplyMovement(Vector3 resultVector)
        {
            player.GetPlayerMovement.InputDirection = resultVector;
        }
        /// <summary>
        /// </summary>
        /// <param name="previousComboState">current Combo State</param>
        protected virtual void OnAttackInput(EComboState previousComboState, EComboState nonImeediateState = EComboState.None)
        {
            player.Attack(previousComboState, nonImeediateState);
        }
        protected virtual void OnSpecialInput()
        {
            if (nextDelayTime_AllowSpecial > Time.time) return;
            nextDelayTime_AllowSpecial = Time.time + GetSpecialDelay;
            Action specialBehaviour = PlayerWeaponManager.CurrentWeapon.GetSpecialBehaviour(entity);
            specialBehaviour.Invoke();
            //GetOwnerFsm.ChangeState(PlayerStateEnum.Parry);
            //entity.GetEntityComponent<PlayerStatCompo>().AddModifier;
        }
        protected virtual void OnDashInput()
        {
            if (nextDelayTime_AllowRoll > Time.time) return;
            nextDelayTime_AllowRoll = Time.time + GetRollDelay;
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
        protected sealed override void OnAttackTrigger(EAttackType eAttackType)
        {
            if (eAttackType == EAttackType.Normal)
            {
                player.GetPlayerDamageCaster.Cast(PlayerWeaponManager.CurrentWeapon.AdditionalNormalDamage, 0, false);
                //player.GetPlayerDamageCaster.Cast();
            }
            if (eAttackType == EAttackType.Heavy)
            {
                player.GetPlayerDamageCaster.Cast(PlayerWeaponManager.CurrentWeapon.AdditionalHeavyDamage, 0, true);
                //player.GetEntityComponent<PlayerStatCompo>().GetStyleMeter.SuccessHit();
            }
        }
    }
}
