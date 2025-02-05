using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade.Enemy.Boss.Reaper
{
    public class ReaperBossAnimatorController : BaseEnemyAnimationController
    {
        private readonly int xVelocity = Animator.StringToHash("XVelocity");
        private readonly int yVelocity = Animator.StringToHash("YVelocity");
        private readonly int zVelocity = Animator.StringToHash("ZVelocity");
        private readonly int yMove = Animator.StringToHash("YMove");
        
        protected override void Start()
        {
            base.Start();
            NavMeshAgent = GetComponentInParent<NavMeshAgent>();
            enemy = GetComponentInParent<BaseEnemy>();

        }
        public void MoveDown()
        {
            Animator.SetFloat(yVelocity , -1);
        }
        public void MoveUp()
        {
            Animator.SetFloat(yVelocity , 1);
        }

        public void SetVelocity(float _x , float _z)
        {
            Animator.SetFloat(xVelocity , _x,0.1f , Time.deltaTime);
            Animator.SetFloat(zVelocity , _z,0.1f , Time.deltaTime);
            
        }
        
        
        
    }
}
