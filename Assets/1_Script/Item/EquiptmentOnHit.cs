using UnityEngine;

namespace Swift_Blade
{
    public class EquiptmentOnHit : BaseEquipment
    {
        [SerializeField] private int cnt = 3;
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

                // todo : implement this
                print("regenHeal");
            }
        }

        public override void Interact()
        {
        }
    }
}
