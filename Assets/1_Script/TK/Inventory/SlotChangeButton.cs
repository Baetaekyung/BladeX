using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public class SlotChangeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject arrow;

        private void Start()
        {
            arrow.SetActive(false);
        }

        public void SetArrowActive(bool isActive)
        {
            arrow.SetActive(isActive);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetArrowActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetArrowActive(false);
        }
    }
}
