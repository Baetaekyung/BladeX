using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Reaper
{
    public class ReaperBoss : BaseEnemy
    {
        [HideInInspector] public ReaperBossAnimatorController _reaperAnimatorController;
        
        private Vector3 lastPosition;
        
        public GameObject weapon;
        
        protected override void Start()
        {
            base.Start();
            _collider = GetComponent<Collider>();
            _reaperAnimatorController = baseAnimationController as ReaperBossAnimatorController;;
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
            GetComponent<Collider>().enabled = _active;
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


        public override void SetDead()
        {
            base.SetDead();
            weapon.AddComponent<EnemyWeapon>();
        }
    }
}
