using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class EquiptmentOnHit : Equipment
    {
        [SerializeField] private int cnt = 3;
        [SerializeField] private int healAmount;

        public override void OnEquipment(bool withoutStat = false)
        {
            base.OnEquipment(withoutStat = false);
            cnt = 0;
        }

        public override void ItemEffect(Player player)
        {
            cnt++;
            if (cnt >= 3)
            {
                cnt = 0;
                var playerHealth = player.GetEntityComponent<PlayerHealth>();
                playerHealth.TakeHeal(healAmount);
            }
        }
    }
}
