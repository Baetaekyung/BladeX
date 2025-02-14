using Swift_Blade.Enemy.Goblin;
using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Goblin
{
    public class GoblinBossAnimatorController : GoblinAnimator
    {
        public bool isRouting;
        private readonly int routingAnimationHash = Animator.StringToHash("Routing");

        private void Update()
        {
            Animator.SetBool(routingAnimationHash, isRouting);
        }

        public void CreateSummon()
        {
            (enemy as GoblinBoss)?.Summon();
        }
    }
}