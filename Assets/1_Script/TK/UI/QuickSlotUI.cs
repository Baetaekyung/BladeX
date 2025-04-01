using System;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class QuickSlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;

        private void Awake()
        {
            icon.color = Color.clear;
        }

        private void Start()
        {
            if (icon.sprite == null)
                icon.color = Color.clear;
            else
                icon.color = Color.white;
        }

        public void SetIcon(Sprite newSprite)
        {   
            icon.color  = newSprite  ? Color.white : Color.clear;
            icon.sprite = newSprite ? newSprite   : null;
        }
    }
}
