using System.Linq.Expressions;
using Swift_Blade.Combat.Projectile;
using Swift_Blade.Feeling;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemBoss : BaseEnemy
    {
        private GolemAnimatorController _golemAnimatorController;
        
       
        protected override void Start()
        {
            base.Start();
            
            _golemAnimatorController =  (baseAnimationController as GolemAnimatorController);
            _golemAnimatorController .target = target;
            
        }
        
        protected override void Update()
        {
            if (baseAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (baseAnimationController.isManualMove && !DetectForwardObstacle())
            {
                attackDestination = transform.position + transform.forward;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    baseAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }

        public override void SetDead()
        {
            base.SetDead();
            
            _golemAnimatorController.SetStone(null);
            
        }

        private void ShakeCam()
        {
            CameraShakeManager.Instance.DoShake(cameraShakeType);
        }
    }
}
