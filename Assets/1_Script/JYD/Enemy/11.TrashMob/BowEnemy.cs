using Swift_Blade.Combat.Caster;


namespace Swift_Blade.Enemy.Bow
{
    public class BowEnemy : BaseEnemy
    {
        protected override void Start()
        {
            base.Start();

            GetComponentInChildren<BowEnemyCaster>().SetTarget(target);
        }
        
        protected override void AddEnemyWeaponCollision()
        {
            if(weapon != null)
                weapon.AddComponent<Bow>();
        }
        
    }
}
