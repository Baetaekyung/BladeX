using Swift_Blade.Boss;
using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade.Boss.Reaper
{
    public class ReaperBossAnimatorController : BossAnimationController
    {
        private readonly int xVelocity = UnityEngine.Animator.StringToHash("XVelocity");
        private readonly int yVelocity = UnityEngine.Animator.StringToHash("YVelocity");
        private readonly int zVelocity = UnityEngine.Animator.StringToHash("ZVelocity");
        private readonly int yMove = UnityEngine.Animator.StringToHash("YMove");
        
        protected override void Start()
        {
            base.Start();
            NavMeshAgent = GetComponentInParent<NavMeshAgent>();
            boss = GetComponentInParent<BaseBoss>();

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
