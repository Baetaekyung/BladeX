using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class SkillMixPopup : PopupUI
    {
        [ContextMenu("Test")]
        public void TestCode()
        {
            PopupManager.Instance.PopUp(PopupType.SkillMix);
        }
    }
}
