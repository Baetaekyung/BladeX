using UnityEngine;

namespace Swift_Blade
{
    public class OneShotItem_IncreaseStat : ItemObject
    {
        [SerializeField] private int value = 1;
        [SerializeField] private StatType targetStat;
        public override void ItemEffect(Player player)
        {
            PlayerHealth playerHealth = player.GetEntityComponent<PlayerHealth>();
            playerHealth.TakeHeal(value);
        }
    }
}
