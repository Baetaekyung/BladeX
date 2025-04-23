using Swift_Blade.Audio;
using Swift_Blade.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerState : BaseStateEntityAnimation<PlayerStateEnum, Player>
    {
        protected readonly Player player;

        protected readonly PlayerMovement playerMovement;
        protected readonly PlayerInput playerInput;
        protected readonly PlayerWeaponManager playerWeaponManager;

        protected static Vector3 anim_inputLocalLerp;
        protected virtual bool BaseAllowAttackInput { get; } = true;
        protected virtual bool BaseAllowSpecialInput { get; } = true;
        protected virtual bool BaseAllowDashInput { get; } = true;
        protected Vector3 GetResultVector => playerInput.CameraRotationOnlyY * playerInput.GetInputDirectionRaw;

        protected float GetSpecialDelay => PlayerWeaponManager.CurrentWeapon.GetSpecialDelay;
        protected float GetRollDelay => PlayerWeaponManager.CurrentWeapon.GetRollDelay;

        private static float nextDelayTime_AllowSpecial;
        private static float nextDelayTime_AllowRoll;
        public static bool parryable;

        protected BasePlayerState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            player = entity;
            playerMovement = player.GetPlayerMovement;
            playerInput = player.GetPlayerInput;
            playerWeaponManager = player.GetEntityComponent<PlayerWeaponManager>();
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
            if (nextDelayTime_AllowSpecial > Time.time && !parryable) return;
            nextDelayTime_AllowSpecial = Time.time + GetSpecialDelay;
            parryable = false;
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
        protected override void OnTrailTrigger(bool active)
        {
            playerWeaponManager.TrailActive = active;
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
            IReadOnlyDictionary<EAttackType, PoolPrefabGameObjectSO> particleDictionary = PlayerWeaponManager.CurrentWeapon.GetParticleDictionary;
            WeaponSO currentWeapon = PlayerWeaponManager.CurrentWeapon;

            bool isStun;
            float damage;

            switch (eAttackType)
            {
                case EAttackType.Normal:
                    damage = currentWeapon.AdditionalNormalDamage;
                    isStun = false;
                    break;
                case EAttackType.Heavy:
                    damage = currentWeapon.AdditionalHeavyDamage;
                    isStun = true;
                    break;
                case EAttackType.RollAttack:
                    damage = currentWeapon.RollAttackDamage;
                    isStun = false;
                    break;  
                default:
                    throw new ArgumentOutOfRangeException($"{eAttackType}");
            }

             if(particleDictionary.TryGetValue(eAttackType, out PoolPrefabGameObjectSO value))
            {
                GameObject particleGameObject = GameObjectPoolManager.Pop(value);
                Transform playerVisualTransform = player.GetPlayerTransform;
                Vector3 particleResultPosition = playerVisualTransform.position + playerVisualTransform.forward * 1.5f;
                Debug.DrawRay(particleResultPosition, Vector3.up, Color.magenta, 3);
                particleGameObject.transform.position = particleResultPosition;
            }

            player.GetPlayerDamageCaster.Cast(damage, 0, isStun);
        }
    }
}
