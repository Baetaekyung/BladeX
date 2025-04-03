using System;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class BlockUI : MonoBehaviour
    {
        private Image  _castableImage;
        private Button _castable;

        private void Awake()
        {
            _castableImage = GetComponent<Image>();
            _castable = GetComponent<Button>();
        }

        private void Start()
        {
            PopupManager.Instance.OnPopUpOpenOrClose += HandleCastable;
        }

        private void HandleCastable()
        {
            if (PopupManager.Instance.IsRemainPopup)
            {
                _castable.interactable = false;
                _castableImage.raycastTarget = false;
            }
            else
            {
                _castable.interactable = true;
                _castableImage.raycastTarget = true;
            }
        }
    }
}
