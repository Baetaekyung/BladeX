using UnityEngine;

namespace Swift_Blade
{
    public abstract class Slot : MonoBehaviour
    {
        public abstract void SetSlotImage(Sprite sprite);
        public abstract bool IsEmptySlot();
        public abstract void SetSlotData<T>(T data);
    }
}
