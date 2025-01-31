using Swift_Blade.Boss;
using UnityEngine;

namespace Swift_Blade.Boss.Reaper
{
    public class ReaperBoss : BaseBoss
    {
        private Collider collider;
        [HideInInspector] public ReaperBossAnimatorController _reaperAnimatorController;
       
        protected override void Start()
        {
            base.Start();
            collider = GetComponent<Collider>();
            _reaperAnimatorController = bossAnimationController as ReaperBossAnimatorController;;
        }
        
        public void MoveInGround()
        {
            _reaperAnimatorController.MoveDown();
            SetCollision(false);
        }
        
        public void MoveOutGround()
        {
          
            NavmeshAgent.Warp(transform.position);
            SetCollision(true);
        }

        public void SetCollision(bool _active)
        {
            NavmeshAgent.enabled = false;
            collider.enabled = false;
        }
        
                        
    }
}
