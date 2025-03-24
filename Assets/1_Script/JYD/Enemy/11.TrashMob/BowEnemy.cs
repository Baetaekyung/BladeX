using Swift_Blade.Combat.Caster;


namespace Swift_Blade.Enemy.Bow
{
    public class BowEnemy : BaseEnemy
    {
        private ICasterAble caster;
        
        protected override void Start()
        {
            base.Start();
            
            caster = GetComponentInChildren<ICasterAble>();
            (caster as BowEnemyCaster).SetTarget(target);
        }
    }
}
