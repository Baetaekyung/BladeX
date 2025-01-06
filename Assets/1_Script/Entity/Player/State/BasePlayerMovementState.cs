using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public abstract class BasePlayerMovementState : BasePlayerState
    {
        protected virtual bool BaseAllowStateChangeToAttack { get; } = true;
        protected virtual bool BaseAllowStateChangeToParry { get; } = true;
        protected virtual bool BaseAllowStateChangeToDash { get; } = true;
        protected readonly Player player;
        protected readonly PlayerMovement playerMovement;
        private static float additionalZValue;
        private Vector3 inputLocalLerp;
        public BasePlayerMovementState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            player = entity;
            playerMovement = entity.GetPlayerMovement;
        }
        public override void Enter()
        {
            base.Enter();
            additionalZValue = 0;
            playerMovement.UseMouseLock = false;
        }
        public override void Update()
        {
            PlayerInput playerInput = player.GetPlayerInput;
            if (Input.GetKeyDown(KeyCode.Mouse0) && BaseAllowStateChangeToAttack)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
            if (Input.GetKeyDown(KeyCode.C) && BaseAllowStateChangeToParry)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Parry);
            if (Input.GetKeyDown(KeyCode.Space) && BaseAllowStateChangeToDash && playerInput.GetInputDirectionRaw.sqrMagnitude > 0.25f && playerMovement.CanRoll)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Dash);
            UI_DebugPlayer.DebugText(2, playerInput.GetInputDirectionRaw.sqrMagnitude > 0.25f, "dashPossible", DBG_UI_KEYS.Keys_PlayerAction);

            //movement
            Vector3 resultInput = playerInput.GetInputDirectionRawRotated.normalized;
            Vector3 resultAnimatorInput = playerInput.GetInputDirectionRawRotated;
            inputLocalLerp = Vector3.MoveTowards(inputLocalLerp, resultAnimatorInput, Time.deltaTime * 8);
            player.GetPlayerMovement.InputDirection = resultInput;

            //animator
            Transform playerTransform = player.GetPlayerRenderer.GetPlayerVisualTrasnform;
            Vector3 inputLocal = playerTransform.InverseTransformDirection(inputLocalLerp);
            Debug.DrawRay(Vector3.zero, inputLocal, Color.red, 0.1f);
            player.GetPlayerAnimator.GetAnimator.SetFloat("X", inputLocal.x);
            additionalZValue = 0;

            player.GetPlayerAnimator.GetAnimator.SetFloat("Z", inputLocal.z + additionalZValue);
        }
        protected override void OnAnimationEndTrigger() => GetOwnerFsm.ChangeState(PlayerStateEnum.Movement);
        protected sealed override void OnSpeedMultiplierDefaultTrigger(float set) => playerMovement.SpeedMultiplierDefault = set;
        protected sealed override void OnMovementSetTrigger(Vector3 value) => playerMovement.SetAdditionalVelocity(value);
        protected override void OnAttackTrigger() => player.GetPlayerDamageCaster.CastDamage();
    }
}
