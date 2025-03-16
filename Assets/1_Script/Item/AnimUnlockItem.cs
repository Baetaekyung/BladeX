using UnityEngine;

namespace Swift_Blade
{
    public class AnimUnlockItem : ItemObject
    {
        [SerializeField] private AttackComboSO animParam;
        public override void ItemEffect()
        {
            Player.Instance.AddCombo(animParam);
        }
    }
}
