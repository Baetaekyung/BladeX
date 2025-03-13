using DG.Tweening;
using Swift_Blade.Feeling;
using Swift_Blade.UI;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    public class StatusPopup : PopupUI
    {
        protected override void Awake()
        {
            base.Awake();
            Player.LevelStat.OnLevelUp += HandleLevelUpPopUp;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Player.LevelStat.OnLevelUp -= HandleLevelUpPopUp;
        }
        
        private void HandleLevelUpPopUp(Player.LevelStat stat)
        {
            PopupManager.Instance.PopUp(PopupType.Status);
        }
        
        public override void Popup()
        {
            _raycaster.enabled = true;
            cG.DOFade(1, _fadeTime)
                .SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    HitStopManager.Instance.EndHitStop();
                    Time.timeScale = 0;
                });
        }
        
        public override void PopDown()
        {
            Time.timeScale = 1;
            
            _raycaster.enabled = false;
            cG.DOFade(0, _fadeTime).SetEase(Ease.OutCirc);
        }
    }
}
