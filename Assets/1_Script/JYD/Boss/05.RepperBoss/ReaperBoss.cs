using Swift_Blade.Boss;
using UnityEngine;

namespace Swift_Blade.Boss.Reaper
{
    public class ReaperBoss : BaseBoss
    {
        private Collider collider;
        [HideInInspector] public ReaperBossAnimatorController _reaperAnimatorController;

        private Vector3 lastPosition;
        
        protected override void Start()
        {
            base.Start();
            collider = GetComponent<Collider>();
            _reaperAnimatorController = bossAnimationController as ReaperBossAnimatorController;;
        }

        protected override void Update()
        {
            base.Update();
            
            SetVelocity();
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
            NavmeshAgent.enabled = _active;
            collider.enabled = _active;
        }
        
        private void SetVelocity()
        {
            Vector3 movement = (transform.position - lastPosition) / Time.deltaTime;
            lastPosition = transform.position;

            Vector3 localVelocity = transform.InverseTransformDirection(movement);

            _reaperAnimatorController.SetVelocity(
                _x: localVelocity.x,
                _z: localVelocity.z
            );
        }
        
                        
    }
}
