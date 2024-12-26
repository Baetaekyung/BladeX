using System;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.FSM.States
{
    public class PlayerParryState : BasePlayerState
    {
        private Player _player;
        private SphereCaster _damageCaster;
        private StatComponent _statCompo;
        private float _currentDuration = 0f;
        private float _parryDuration; //나중에 스텟으로 받으면 좋을 것 같다.
        
        public PlayerParryState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            _player = entity;
            _damageCaster = _player.GetComponent<SphereCaster>();
            _statCompo = _player.GetComponent<StatComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("Enter to Parry state");
            
            _parryDuration = _statCompo.GetStatByType(StatType.PARRYDURATION).Value; //Todo: 나중에 GetStat(ParryStat)으로 패링 지속시간 관리하기
            Debug.Log("Parry Duration: " + _parryDuration);
            
            _currentDuration = 0f;
        }

        public override void Update()
        {
            base.Update();

            if (_currentDuration < _parryDuration)
            {
                _currentDuration += Time.deltaTime;

                if (_damageCaster.CastDamage())
                    ParryOnHitHandler();
            }
            //패링 지속시간 종료
            else
                GetOwnerFsm.ChangeState(PlayerStateEnum.Movement);
        }

        public override void Exit()
        {
            Debug.Log("Exit from Parry state");
            _damageCaster.OnCastDamageEvent.RemoveListener(ParryOnHitHandler);

            base.Exit();
        }

        private void ParryOnHitHandler()
        {
            Debug.Log("공격에 맞아서 패링 공격 발동");
            GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        }
    }
}
