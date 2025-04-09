using Swift_Blade.FSM.States;

namespace Swift_Blade.Enemy.Boss
{
    public class SorcererBoss : BaseEnemy
    {
        public void ResetParry()
        {
            BasePlayerState.parryable = true;
        }
    }
}
