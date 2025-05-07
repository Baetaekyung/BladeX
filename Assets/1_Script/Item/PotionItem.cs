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
                PopupManager.Instance.LogInfoBox("ü���� �����Ͽ� ������ ����� �� �����ϴ�.");
                return false;
            }

            return true;
        }
    }
}
