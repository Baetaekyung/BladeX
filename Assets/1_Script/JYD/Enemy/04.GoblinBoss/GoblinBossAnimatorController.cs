using Swift_Blade.Enemy.Golbin;
using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Golbin
{
    public class GoblinBossAnimatorController : GoblinAnimator
    {
        public bool isRouting;
        
        private int routingAnimationHash = Animator.StringToHash("Routing");
        
        private void Update()
        {
            Animator.SetBool(routingAnimationHash,isRouting);
        }
                
        public void CreateSummon()
        {
            (enemy as GoblinBoss)?.Summon();
        }
               
    }
}
