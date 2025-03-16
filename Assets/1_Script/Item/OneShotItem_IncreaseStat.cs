using UnityEngine;

namespace Swift_Blade
{
    public class OneShotItem_IncreaseStat : ItemObject
    {
        [SerializeField] private int value = 1;
        [SerializeField] private StatType targetStat;
        public override void ItemEffect(Player player)
        {
            // PlayerStatCompo psc = Player.Instance.GetEntityComponent<PlayerStatCompo>();
            // StatSO targetStatSO = psc.GetStat(targetStat);
            // psc.IncreaseBaseValue(targetStatSO, value);
            Debug.Log("Item Use");
        }
    }
}
