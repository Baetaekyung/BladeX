using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Golem
{
    public class GolemBoss : BaseEnemy
    {
        protected override void Update()
        {
            if (baseAnimationController.isManualRotate)
            {
                FactToTarget(target.position);
            }

            if (baseAnimationController.isManualMove)
            {
                attackDestination = transform.position + transform.forward;

                transform.position = Vector3.MoveTowards(transform.position, attackDestination, 
                    baseAnimationController.AttackMoveSpeed * Time.deltaTime);
            }
        }

        private void ShakeCam()
        {
            CameraShakeManager.Instance.DoShake(cameraShakeType);
        }
    }
}
