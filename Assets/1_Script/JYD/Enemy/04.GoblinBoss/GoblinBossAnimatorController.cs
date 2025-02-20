using Swift_Blade.Enemy.Goblin;

namespace Swift_Blade.Enemy.Boss.Goblin
{
    public class GoblinBossAnimatorController : GoblinAnimator
    {
        public void CreateSummon()
        {
            (enemy as GoblinBoss)?.Summon();
        }
    }
}