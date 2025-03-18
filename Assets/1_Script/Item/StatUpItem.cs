using UnityEngine;

namespace Swift_Blade
{
    public class StatUpItem : ItemObject
    {
        public SerializableDictionary<StatType, float> upgradeStat
            = new SerializableDictionary<StatType, float>();

        [SerializeField] private StatSO statSO;
        public override void ItemEffect(Player player)
        {
            foreach(var stat in upgradeStat)
            {
                Debug.Log("아이템 사용 " + $"{stat.Key} , {stat.Value}");
                player.GetEntityComponent<PlayerStatCompo>().AddModifier(stat.Key, "Skillming", stat.Value);
            }
        }
    }
}
