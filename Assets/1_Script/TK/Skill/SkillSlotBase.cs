using System;
using UnityEngine;
using Swift_Blade.Skill;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public abstract class SkillSlotBase : MonoBehaviour,
        IPointerDownHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerMoveHandler
    {
        public static Action<Vector2, SkillData> OnPointerEnterAction;
        
        public abstract void SetSlotImage(Sprite sprite);
        public abstract bool IsEmptySlot();
        public abstract void SetSlotData(SkillData data);


        #region Raycast Handlers

        public abstract void OnPointerDown(PointerEventData eventData);
        public abstract void OnPointerEnter(PointerEventData eventData);
        public abstract void OnPointerExit(PointerEventData eventData);
        public abstract void OnPointerMove(PointerEventData eventData);

        #endregion
    }
}
