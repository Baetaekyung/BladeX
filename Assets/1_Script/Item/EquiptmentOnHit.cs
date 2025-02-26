using UnityEngine;

namespace Swift_Blade
{
    public class EquiptmentOnHit : BaseEquipment
    {
        [SerializeField] private int cnt = 3;
        [SerializeField] private int healAmount;
        public override void OnEquipment()
        {
            base.OnEquipment();
            cnt = 0;
        }
        public override void ItemEffect()
        {
            cnt++;
            if (cnt >= 3)
            {
                cnt = 0;
                PlayerHealth playerHealth = Player.Instance.GetEntityComponent<PlayerHealth>();
                playerHealth.TakeHeal(healAmount);
            }
        }

        public override void Interact()
        {
        }
    }
}
