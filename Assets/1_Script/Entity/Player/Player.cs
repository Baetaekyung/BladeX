using Swift_Blade.FSM;
using Swift_Blade.FSM.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum PlayerStateEnum
    {
        Idle,
        Movement,
        Attack,
        Dash,
        Parry
    }
    public class Player : Entity
    {
        [Header("Debug Fields")]
        [SerializeField] private float debug_cameraDistance;
        [SerializeField] private AnimationTriggers animEndTrigger;

        [Header("Debug_Params")]
        [SerializeField] private AnimationParameterSO anim_idle;
        [SerializeField] private AnimationParameterSO anim_move;
        [SerializeField] private AnimationParameterSO anim_jumpAttack;
        [SerializeField] private AnimationParameterSO anim_attack1;
        [SerializeField] private AnimationParameterSO anim_parry;

        [Header("Combo")]
        //should be serializeable struct later
        [SerializeField] private AnimationParameterSO[] comboParamHash;
        [SerializeField] private Vector3[] comboForceList;
        [SerializeField] private float[] periods;
        
        [Header("Parring")]
        public bool IsParryState { get; set; }
        
        public IReadOnlyList<AnimationParameterSO> GetComboHashAtk => comboParamHash;
        public IReadOnlyList<Vector3> GetComboForceList => comboForceList;
        public IReadOnlyList<float> GetPeriods => periods;

        #region PlayerComponentGetter
        public PlayerCamera GetPlayerCamera => GetEntityComponent<PlayerCamera>();
        public PlayerMovement GetPlayerMovement => GetEntityComponent<PlayerMovement>();
        public PlayerInput GetPlayerInput => GetEntityComponent<PlayerInput>();
        public PlayerRenderer GetPlayerRenderer => GetEntityComponent<PlayerRenderer>();
        public PlayerAnimator GetPlayerAnimator => GetEntityComponent<PlayerAnimator>();
        public PlayerDamageCaster GetPlayerDamageCaster => GetEntityComponent<PlayerDamageCaster>();
        #endregion

        public static event Action Debug_Updt;
        private readonly FiniteStateMachine<PlayerStateEnum> playerStateMachine = new();
        protected override void Awake()
        {
            base.Awake();
            Animator playerAnimator = GetPlayerRenderer.GetPlayerAnimator.GetAnimator;
            //playerStateMachine.AddState(PlayerStateEnum.Idle, new PlayerIdleState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_idle));
            playerStateMachine.AddState(PlayerStateEnum.Movement, new PlayerMoveState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_move));
            playerStateMachine.AddState(PlayerStateEnum.Attack, new PlayerAttackState(playerStateMachine, playerAnimator, this, animEndTrigger, null));
            playerStateMachine.AddState(PlayerStateEnum.Dash, new PlayerDashState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_idle));
            playerStateMachine.AddState(PlayerStateEnum.Parry, new PlayerParryState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_parry));
            playerStateMachine.SetStartState(PlayerStateEnum.Movement);
        }
        private void Update()
        {
            playerStateMachine.UpdateState();
            Debug_Updt?.Invoke();
            void UpdateDebugUI()
            {
                if (Input.GetKeyDown(KeyCode.F1))
                    UI_DebugPlayer.Instance.ShowDebugUI = !UI_DebugPlayer.Instance.ShowDebugUI;
                UI_DebugPlayer.DebugText(0, playerStateMachine.CurrentState.ToString(), "cs", DBG_UI_KEYS.Keys_PlayerAction);
            }
            UpdateDebugUI();
            void ProcessInput()
            {
                if (Input.GetKeyDown(KeyCode.L))
                    GetPlayerMovement.LockOnEnemy = !GetPlayerMovement.LockOnEnemy;
            }
            ProcessInput();
        }

    }
}
