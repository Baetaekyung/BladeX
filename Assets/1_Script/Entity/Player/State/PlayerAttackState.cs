using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerAttackState : BasePlayerState
    {
        protected override bool BaseAllowParryInput => false;

        private readonly PlayerRenderer playerRenderer;
        private AttackComboSO currentAttackComboSO;

        private bool allowListening;
        private bool allowNextAttack;
        private bool inputBuffer;

        private int currentIndex;
        private float deadPeriod;

        private bool IsDeadPeriodOver => deadPeriod > Time.time;
        //private readonly int maxIdx;
        //private bool IsIndexValid => currentIdx < maxIdx;
        /// <summary>
        /// todo : bad name
        /// </summary>
        public EComboState PreviousComboState { get; set; }
        private readonly List<EComboState> comboStateHistory = new(5);
        public PlayerAttackState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null)
            : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            playerRenderer = player.GetPlayerRenderer;
            //comboParamHash = player.GetComboHashAtk;
            //comboForceList = player.GetComboForceList;
            //delayArr = player.GetPeriods;
            //maxIdx = comboParamHash.Count - 1;
            //dashAttack = anim_dashAttack;
            Player.Debug_Updt += () =>
            {
                UI_DebugPlayer.DebugText(2, IsDeadPeriodOver, "dpover", DBG_UI_KEYS.Keys_PlayerAction);
                UI_DebugPlayer.DebugText(3, deadPeriod, "deadPeriod", DBG_UI_KEYS.Keys_PlayerAction);
                UI_DebugPlayer.DebugText(4, Time.time, "time", DBG_UI_KEYS.Keys_PlayerAction);
            };
        }

        public override void Enter()
        {
            base.Enter();
            comboStateHistory.Clear();
            bool matchFound = false;
            foreach (var item in player.GetComboList)
            {
                if (item.GetComboes.Count > currentIndex)
                    if (item.IsMatchFirstIndex(PreviousComboState, out AttackComboSO comboData))
                    {
                        //currentAttackComboSO = comboData;
                        matchFound = true;
                        break;
                    }
            }
            if (matchFound)
            {
                comboStateHistory.Add(PreviousComboState);
                bool mouseMove = true;
                Vector3 direction = mouseMove ?
                    player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                    player.GetPlayerInput.GetInputDirectionRawRotated;
                playerRenderer.LookAtDirection(direction);
                playerMovement.UseMouseLock = true;

                ComboAttack();
            }
            else
                Debug.Log("no match, no combo");
        }
        public override void Update()
        {
            base.Update();
            if (inputBuffer && allowNextAttack)// && IsIndexValid)// && !dashAttackTrigger)
                ComboAttack();
        }
        protected override void OnAttackInput(EComboState currentState)
        {
            if (allowListening)
                inputBuffer = true;
        }

        private void ComboAttack()
        {
            if (IsDeadPeriodOver)
                currentIndex++;
            else
                currentIndex = 0;

            inputBuffer = false;
            allowListening = false;
            allowNextAttack = false;
            //deadPeriod = currentAttackComboSO.GetComboes[currentIndex].GetPeriod + Time.time;

            AnimationParameterSO param = currentAttackComboSO.GetComboes[currentIndex].GetAnimParam;// comboParamHash[currentIdx];
            PlayAnimation(param);
        }
        protected override void OnAnimationEndTriggerListen() => allowListening = true;
        protected override void OnAnimationEndableTrigger() => allowNextAttack = true;
        protected override void OnForceEventTrigger(float force)
        {
            Vector3 result = default;// currentAttackComboSO.GetComboes[currentIndex].GetComboForce;// comboForceList[currentIdx] * force;
            playerMovement.AddForceFacingDirection(result);
        }
        public override void Exit()
        {
            playerMovement.SpeedMultiplierDefault = 1;
            //playerMovement.UseMouseLock = false;
            base.Exit();
        }
    }
}
