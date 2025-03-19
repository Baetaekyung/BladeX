using DG.Tweening;
using UnityEngine;

namespace Swift_Blade.UI
{
    public class StartGameBtn : BaseButton
    {
        [SerializeField] private SceneManagerSO sceneManagerSo;
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
            
            sceneManagerSo.LoadScene("Menu");
        }
    }
}
