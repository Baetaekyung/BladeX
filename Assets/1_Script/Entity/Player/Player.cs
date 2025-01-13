using Swift_Blade.FSM;
using Swift_Blade.FSM.States;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum PlayerStateEnum
    {
        Move,
        Attack,
        Roll,
        Parry
    }
    public class Player : Entity
    {
        [Header("Debug_Params")]
        [SerializeField] private AnimationTriggers animEndTrigger;
        [SerializeField, Space(10)] private AnimationParameterSO anim_idle;
        [SerializeField] private AnimationParameterSO anim_move;
        [SerializeField] private AnimationParameterSO anim_parry;
        [SerializeField] private AnimationParameterSO anim_roll;
        [SerializeField] private AnimationParameterSO anim_rollAttack;

        [Header("Combo")]
        [SerializeField] protected AttackComboSO[] comboList;
        public IReadOnlyList<AttackComboSO> GetComboList => comboList;
        //[SerializeField] private AnimationParameterSO[] comboParamHash;
        //[SerializeField] private Vector3[] comboForceList;
        //[SerializeField] private float[] periods;
        //
        //public IReadOnlyList<AnimationParameterSO> GetComboHashAtk => comboParamHash;
        //public IReadOnlyList<Vector3> GetComboForceList => comboForceList;
        //public IReadOnlyList<float> GetPeriods => periods;
        public bool IsParryState { get; set; }

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
        private PlayerAttackState playerAttackState;
        protected override void Awake()
        {
            base.Awake();
            Animator playerAnimator = GetPlayerRenderer.GetPlayerAnimator.GetAnimator;
            playerStateMachine.AddState(PlayerStateEnum.Move,   new PlayerMoveState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_move));
            playerAttackState =                                 new PlayerAttackState(playerStateMachine, playerAnimator, this, animEndTrigger, null);
            playerStateMachine.AddState(PlayerStateEnum.Attack, playerAttackState);
            playerStateMachine.AddState(PlayerStateEnum.Roll,   new PlayerRollState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_roll));
            playerStateMachine.AddState(PlayerStateEnum.Parry,  new PlayerParryState(playerStateMachine, playerAnimator, this, animEndTrigger, anim_parry));
            playerStateMachine.SetStartState(PlayerStateEnum.Move);
        }
        private void Update()
        {
            playerStateMachine.UpdateState();

            Debug_Updt?.Invoke();
            if (Input.GetKeyDown(KeyCode.F1))
                UI_DebugPlayer.Instance.ShowDebugUI = !UI_DebugPlayer.Instance.ShowDebugUI;
            UI_DebugPlayer.DebugText(0, playerStateMachine.CurrentState.ToString(), "cs", DBG_UI_KEYS.Keys_PlayerAction);
        }
        public void Attack(EComboState previousState)
        {
            playerAttackState.PreviousComboState = previousState;
            playerStateMachine.ChangeState(PlayerStateEnum.Attack);
        }

        public PlayerStateEnum GetCurrentState() => playerStateMachine.GetState();
        
    }
}
