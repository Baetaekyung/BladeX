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

        public override bool CanUse()
        {
            float maxHp = Player.Instance.GetEntityComponent<PlayerHealth>().maxHealth;

            bool isPlayerFullHP = Mathf.Approximately(maxHp, PlayerHealth.CurrentHealth);

            if (isPlayerFullHP)
            {
                PopupManager.Instance.LogInfoBox("체력이 가득하여 포션을 사용할 수 없습니다.");
                return false;
            }

            return true;
        }
    }
}
