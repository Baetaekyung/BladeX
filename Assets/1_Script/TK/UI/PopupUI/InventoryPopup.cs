using System;
using DG.Tweening;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class InventoryPopup : PopupUI
    {
        private void OnDisable()
        {
            InventoryManager.Instance.UpdateAllSlots();
        }
    }
}
