using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class BasePlayerMovementState : BasePlayerState
    {
        protected virtual bool BaseAllowStateChangeToAttack { get; } = true;
        protected readonly Player player;
        protected readonly PlayerMovement playerMovement;
        private static float additionalZValue;
        public BasePlayerMovementState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            player = entity;
            playerMovement = entity.GetPlayerMovement;
        }
        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && BaseAllowStateChangeToAttack)
                GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
            if (Input.GetKeyDown(KeyCode.Space))
                GetOwnerFsm.ChangeState(PlayerStateEnum.Dash);

            Transform playerTransform = player.GetPlayerRenderer.GetPlayerVisualTrasnform;
            Vector3 input = player.GetPlayerInput.InputDirectionRaw;
            Vector3 inputNormalized = input.normalized;

            //movement
            Quaternion playerCameraRotation = player.GetPlayerCamera.GetResultQuaternion;
            Vector3 resultInput = playerCameraRotation * inputNormalized;
            Vector3 resultInputAnimator = playerCameraRotation * input;
            Debug.DrawRay(player.transform.position, resultInput);
            player.GetPlayerMovement.InputDirection = resultInput;

            //animator
            Vector3 inputLocal = playerTransform.InverseTransformDirection(resultInputAnimator);
            player.GetPlayerAnimator.GetAnimator.SetFloat("X", inputLocal.x);
            if (inputLocal.z >= 0.3f)
                additionalZValue = Mathf.MoveTowards(additionalZValue, 1, Time.deltaTime * 1.5f);
            else
                additionalZValue = 0;
            player.GetPlayerAnimator.GetAnimator.SetFloat("Z", inputLocal.z + additionalZValue);
            player.GetPlayerMovement.SpeedMultiplierForward = Mathf.Max(additionalZValue + 0.5f, 1);
            //UI_DebugPlayer.Instance.GetList[6].text = additionalZValue.ToString();
            //Debug.DrawRay(transform.position, input, Color.red);
            //Debug.DrawRay(transform.position, inputLocal, Color.blue)
        }
        protected sealed override void OnSpeedMultiplierDefaultTrigger(float set) => playerMovement.SpeedMultiplierDefault = set;
        protected sealed override void OnMovementSetTrigger(Vector3 value) => playerMovement.SetAdditionalVelocity(Vector3.zero);
    }
}
