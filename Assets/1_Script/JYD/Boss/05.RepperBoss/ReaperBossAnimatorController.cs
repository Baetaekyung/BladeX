using Swift_Blade.Boss;
using UnityEngine;
using UnityEngine.AI;

namespace Swift_Blade.Boss.Reaper
{
    public class ReaperBossAnimatorController : BossAnimationController
    {
        protected override void Start()
        {
            base.Start();
            NavMeshAgent = GetComponentInParent<NavMeshAgent>();
            boss = GetComponentInParent<BaseBoss>();
            
            

        }
    }
}
