using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class InGameUIManager : MonoSingleton<InGameUIManager>
    {
        [field: SerializeField] public GameObject BossHealthBarUI { get; private set; }

        [SerializeField] private SceneManagerSO sceneManagerSo;

        private void Start()
        {
            sceneManagerSo.SceneEnterEvent += Execute;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            sceneManagerSo.SceneEnterEvent -= Execute;
        }

        private void Execute()
        {
            GetComponentsInChildren<IUIExecute>().ToList().ForEach(x =>
            {
                x.Execute();
            });
        }
        
        [ContextMenu("Test")]
        public void EnableBoss()
        {
            EnableBossUIs(true);
        }
        
        public void EnableBossUIs(bool enable)
        {
            if (enable)
            {
                BossHealthBarUI.gameObject.SetActive(true);
                BossHealthBarUI.GetComponent<RectTransform>().DOAnchorPosY(-75, 0.7f)
                    .SetEase(Ease.OutBounce);
            }
            else
            {
                BossHealthBarUI.GetComponent<RectTransform>().DOAnchorPosY(110, 0.7f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => BossHealthBarUI.gameObject.SetActive(false));
            }
        }
    }
}
