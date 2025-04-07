using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class PotionItem : ItemObject
    {
        [SerializeField] private float increaseAmount;

        public override void ItemEffect(Player player)
        {
            player.GetEntityComponent<PlayerHealth>().TakeHeal(increaseAmount);
        }
    }
}
