using Swift_Blade.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public abstract class SkillSlotBase : MonoBehaviour, IPointerDownHandler
    {
        public abstract void SetSlotImage(Sprite sprite);
        public abstract bool IsEmptySlot();
        public abstract void SetSlotData(SkillData data);
        
        public abstract void OnPointerDown(PointerEventData eventData);
    }
}
