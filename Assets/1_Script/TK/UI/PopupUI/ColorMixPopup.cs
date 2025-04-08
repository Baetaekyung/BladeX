using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class ColorMixPopup : PopupUI
    {
        [ContextMenu("Test")]
        public void PopupForTest()
        {
            PopupManager.Instance.PopUp(PopupType.SkillMix);
        }
    }
}
