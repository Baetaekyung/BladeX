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
            {
                icon.color = Color.clear;
            }
            else
            {
                icon.color = Color.white;
            }
        }

        public void SetIcon(Sprite newSprite)
        {
            if (newSprite == null)
            {
                icon.sprite = null;
                icon.color = Color.clear;
                return;
            }
            
            icon.color = Color.white;
            icon.sprite = newSprite;
        }
    }
}
