using Swift_Blade.Combat;
using UnityEngine;

namespace Swift_Blade.FSM.States
{
    public class PlayerParryState : BasePlayerState
    {
        protected override bool BaseAllowParryInput => false;
        protected override bool BaseAllowDashInput => false;
        protected override bool BaseAllowAttackInput => false;
        private readonly PlayerRenderer playerRenderer;
        
        private float canParryTime;
        private float parryTimer;
        
        private PlayerParryController parryController;
        
        public PlayerParryState(FiniteStateMachine<PlayerStateEnum> stateMachine, Animator animator, Player entity, AnimationTriggers animTrigger, AnimationParameterSO animParamSO = null) : base(stateMachine, animator, entity, animTrigger, animParamSO)
        {
            playerRenderer = player.GetPlayerRenderer;
            parryController = player.GetPlayerParryController;
        }

        public override void Enter()
        {
            base.Enter();
            bool mouseMove = true;
            
            Vector3 direction = mouseMove == true ?
                player.GetPlayerInput.GetMousePositionWorld - playerMovement.transform.position :
                player.GetPlayerInput.GetInputDirectionRawRotated;
            
            playerRenderer.LookAtDirection(direction);
            
            playerMovement.AllowInputMove = false;
            
            
            canParryTime = parryController.ParryTime;
            parryTimer = 0;
            
            parryController.SetParry(true);
        }

        public override void Update()
        {
            base.Update();

            parryTimer += Time.deltaTime;
            
            if (parryTimer >= canParryTime)
            {
                if(parryController.CanParry())
                    parryController.SetParry(false);
            }
        }

        public override void Exit()
        {
            parryController.SetParry(false);
            playerMovement.AllowInputMove = true;
                        
            base.Exit();
        }
    }
}
