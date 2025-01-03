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
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && BaseAllowStateChangeToAttack)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
            if (Input.GetKeyDown(KeyCode.Space))
                GetOwnerFsm.ChangeState(PlayerStateEnum.Dash);
            if (Input.GetKeyDown(KeyCode.C) && BaseAllowStateChangeToParry)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Parry);
            if (Input.GetKeyDown(KeyCode.Space) && BaseAllowStateChangeToDash)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Dash);

            PlayerInput playerInput = player.GetPlayerInput;

            //movement
            Vector3 resultInput = playerInput.GetInputDirectionRawRotated.normalized;
            Vector3 resultAnimatorInput = playerInput.GetInputDirectionRawRotated;
            inputLocalLerp = Vector3.MoveTowards(inputLocalLerp, resultAnimatorInput, Time.deltaTime * 8);
            player.GetPlayerMovement.InputDirection = resultInput;

            //animator
            Transform playerTransform = player.GetPlayerRenderer.GetPlayerVisualTrasnform;
            Vector3 inputLocal = playerTransform.InverseTransformDirection(inputLocalLerp);
            Debug.DrawRay(Vector3.zero, inputLocal, Color.red, 0.1f);
            UI_DebugPlayer.DebugText(0, inputLocal, "inputLocal", DBG_UI_KEYS.Keys_PlayerAction);
            //UI_DebugPlayer.DebugText(2, inputLocal.magnitude, "inputLocalMag", DBG_UI_KEYS.Keys_PlayerAction);
            player.GetPlayerAnimator.GetAnimator.SetFloat("X", inputLocal.x);
            //if (inputLocal.z >= 0.3f)
            //    additionalZValue = Mathf.MoveTowards(additionalZValue, 1, Time.deltaTime * 1.5f);
            //else
            //    additionalZValue = 0;

            additionalZValue = 0;

            player.GetPlayerAnimator.GetAnimator.SetFloat("Z", inputLocal.z + additionalZValue);
            player.GetPlayerMovement.SpeedMultiplierForward = 1;// Mathf.Max(additionalZValue + 0.5f, 1);

            //UI_DebugPlayer.Instance.GetList[6].text = additionalZValue.ToString();
            //Debug.DrawRay(transform.position, input, Color.red);
            //Debug.DrawRay(transform.position, inputLocal, Color.blue)
        }
        protected sealed override void OnSpeedMultiplierDefaultTrigger(float set) => playerMovement.SpeedMultiplierDefault = set;
        protected sealed override void OnMovementSetTrigger(Vector3 value) => playerMovement.SetAdditionalVelocity(Vector3.zero);
    }
}
