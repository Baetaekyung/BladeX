using UnityEngine;

namespace Swift_Blade
{
    public abstract class ItemObject : MonoBehaviour
    {
        public abstract void ItemEffect(Player player); //아이템 효과
        public virtual bool CanUse() => true;
    }
}
