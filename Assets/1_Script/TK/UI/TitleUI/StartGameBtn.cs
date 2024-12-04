using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class StartGameBtn : BaseButton
    {
        protected override void ClickEvent()
        {

            #region Check is animationUI

            //Check is hoverable ui
            if (TryGetComponent(typeof(HoverUI), out var component))
            {
                if (component is HoverUI hoverUI)
                    hoverUI.SetHovering(false);
            }

            #endregion
            
            Debug.Log("GameStart");
        }
    }
}
