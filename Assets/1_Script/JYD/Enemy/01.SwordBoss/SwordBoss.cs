using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Sword
{
    public class SwordBoss : BaseEnemy
    {
        public GameObject sword;

        public override void DeadEvent()
        {
            base.DeadEvent();
            
            sword.AddComponent<EnemyWeapon>();
        }
        
    }
}
