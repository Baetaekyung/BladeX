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
        public override void EquipmentEffect()
        {
            cnt++;
            if (cnt >= 3)
            {
                cnt = 0;
                PlayerHealth playerHealth = default;
                playerHealth.TakeHeal(healAmount);
            }
        }

        public override void Interact()
        {
        }
    }
}
