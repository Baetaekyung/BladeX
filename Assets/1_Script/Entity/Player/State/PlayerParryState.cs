using Swift_Blade.Combat;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerParryState : BasePlayerState
    {
        protected override bool BaseAllowParryInput => false;
        protected override bool BaseAllowDashInput => false;
        protected override bool BaseAllowAttackInput => false;
        private readonly PlayerHealth _playerHealthCompo;
        private readonly PlayerRenderer playerRenderer;
        
        //parry의 판정을 검사할 타임과 타이머
        private float canParryTime;
        private float parryTimer;
        
        private PlayerParryController parryController;
        
        /*private float _currentDuration = 0f;
        private float _parryDuration;*/
        
        public PlayerParryState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            playerRenderer = player.GetPlayerRenderer;
            parryController = player.GetPlayerParryController;
            
            _playerHealthCompo = player.GetComponentInChildren<PlayerHealth>();
        }

        public override void Enter()
        {
            base.Enter();
            bool mouseMove = true;
            
            Vector3 direction = mouseMove == true ?
                player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                player.GetPlayerInput.GetInputDirectionRawRotated;
            
            playerRenderer.LookAtDirection(direction);
            
            player.IsParryState = true;
            playerMovement.AllowInputMove = false;
            
            //_parryDuration = 0.4f;
            _playerHealthCompo.OnHitEvent.AddListener(ParryOnHitByVicinityHandler);
            
            //ParryTime/ParryTimer 초기화
            canParryTime = parryController.ParryTime;
            parryTimer = 0;

            parryController.SetParry(true);
        }

        public override void Update()
        {
            base.Update();

            parryTimer += Time.deltaTime;
            
            if (parryTimer >= canParryTime)//Parry이 타임이 다 됐는데도 Parry가 가능하면 Parry끄기.
            {
                if(parryController.CanParry())
                    parryController.SetParry(false);
            }
            
            
            //_currentDuration += Time.deltaTime;

            /*if (_currentDuration >= _parryDuration)
            {
                _playerHealthCompo.OnHitEvent.RemoveListener(ParryOnHitByVicinityHandler);
            }*/


        }

        public override void Exit()
        {
            _playerHealthCompo.OnHitEvent.RemoveListener(ParryOnHitByVicinityHandler);
            
           
            //_currentDuration = 0f;
            parryController.SetParry(false);
            player.IsParryState = false;
            playerMovement.AllowInputMove = true;
            
            base.Exit();
        }

        private void DoActionFeeling() //패링 성공시 비주얼 효과들
        {
            // HitStopManager.Instance.DoHitStop(_data.hitStopData);
            // CameraShakeManager.Instance.DoShake(CameraShakeType.ParryShake);
            // CameraFocusManager.Instance.DoFocus(_data.cameraFocusData);
            // PostProcessManager.Instance.DoPostProcessing(_data.parryPostProcessing, 0.4f);
            // _data.parryLight.SetActive(true);
        }
        
        private void ParryOnHitByVicinityHandler(ActionData actionData)
        {
            DoActionFeeling();
            //StyleMeter.Instance.RaiseMeterPercent(100f); //100퍼센트 증가 (보류)
            
            //LookAtTarget(actionData);

            //GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        }
        
        // private void ParryOnHitByRangeHandler(ActionData actionData) 보류
        // {
        //     DoActionFeeling();
        //
        //     StyleMeter.Instance.RaiseMeterPercent(50f); //50퍼센트 증가
        //     LookAtTarget(actionData);
        //
        //     //Todo:발사체 만들어서 direction으로 날리기. (플레이어에 원거리 공격체 게임오브젝트 있어야함)
        //     
        //     GetOwnerFsm.ChangeState(PlayerStateEnum.Attack);
        // }
        private void LookAtTarget(ActionData actionData)
        {
            Vector3 dir = actionData.dealer.transform.position - playerRenderer.GetPlayerVisualTrasnform.position;
            playerRenderer.LookAtDirection(dir);
            //Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
            //lookRotation.x = 0; //y축만 회전하도록
            //lookRotation.z = 0; //y축만 회전하도록
        }
        
                
        
    }
}
