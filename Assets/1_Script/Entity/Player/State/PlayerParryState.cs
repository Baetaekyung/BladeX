using System;
using Swift_Blade.Feeling;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.FSM.States
{
    public class PlayerParryState : BasePlayerState
    {
        private Transform _visual;
        
        private readonly Player _player;
        private readonly PlayerParryData _data;
        private readonly PlayerHealth _playerHealthCompo;
        private readonly PlayerMovement _movementCompo;
        private readonly StatComponent _statCompo;
        
        private float _currentDuration = 0f;
        private float _parryDuration;
        
        public PlayerParryState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            _player = entity;
            _visual = GameObject.Find("Vis").transform;
            _movementCompo = _player.GetPlayerMovement;
            
            _data = _player.GetComponentInChildren<PlayerParryData>();
            _playerHealthCompo = _player.GetComponentInChildren<PlayerHealth>();
            _statCompo = _player.GetComponentInChildren<StatComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            
            _movementCompo.AllowInputMoving = false;
            
            _parryDuration = _statCompo.GetStatByType(StatType.PARRYDURATION).Value;
            _playerHealthCompo.OnHitByVicinityEvent += ParryOnHitByVicinityHandler;
            _playerHealthCompo.OnHitByRangeEvent += ParryOnHitByRangeHandler;
        }

        public override void Update()
        {
            base.Update();

            _currentDuration += Time.deltaTime;

            if (_currentDuration >= _parryDuration)
            {
                StyleMeter.Instance.DowngradeMeterRank(); //패링 실패시 랭크가 내려가도록
                GetOwnerFsm.ChangeState(PlayerStateEnum.Idle);
            }
        }

        public override void Exit()
        {
            _currentDuration = 0f;
            
            _movementCompo.AllowInputMoving = true;
            _playerHealthCompo.OnHitByVicinityEvent -= ParryOnHitByVicinityHandler;
            _playerHealthCompo.OnHitByRangeEvent -= ParryOnHitByRangeHandler;
            
            base.Exit();
        }

        private void DoActionFeeling() //패링 성공시 비주얼 효과들
        {
            HitStopManager.Instance.DoHitStop(_data.hitStopData);
            CameraShakeManager.Instance.DoShake(CameraShakeType.ParryShake);
            CameraFocusManager.Instance.DoFocus(_data.cameraFocusData);
            PostProcessManager.Instance.DoPostProcessing(_data.parryPostProcessing, 0.4f);
            _data.parryLight.SetActive(true);
        }
        
        private void ParryOnHitByVicinityHandler(ActionData actionData)
        {
            DoActionFeeling();
            StyleMeter.Instance.RaiseMeterPercent(100f); //100퍼센트 증가
            
            LookAtTarget(actionData);

            Debug.Log("근접 공격에 맞아서 패링 공격 발동");
            GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        }
        
        private void ParryOnHitByRangeHandler(ActionData actionData)
        {
            DoActionFeeling();

            StyleMeter.Instance.RaiseMeterPercent(50f); //50퍼센트 증가
            LookAtTarget(actionData);

            Debug.Log("원거리 공격에 맞아서 패링 공격 발동");
            //Todo:발사체 만들어서 direction으로 날리기. (플레이어에 원거리 공격체 게임오브젝트 있어야함)
            
            GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        }
        
        private void LookAtTarget(ActionData actionData)
        {
            Vector3 dir = actionData.dealer.transform.position - _visual.position;
            
            Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
            lookRotation.x = 0; //y축만 회전하도록
            lookRotation.z = 0; //y축만 회전하도록
            
            _visual.rotation = lookRotation;
        }
    }
}
