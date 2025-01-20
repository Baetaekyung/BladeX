using UnityEngine;

namespace Swift_Blade.Boss.Goblin
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
            (boss as GoblinBoss)?.Summon();
        }

        

       
    }
}
