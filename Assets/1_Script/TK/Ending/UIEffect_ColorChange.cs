using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class UIEffect_ColorChange : MonoBehaviour
    {
        [SerializeField] private Image[] effs;
        [SerializeField] private CanvasGroup cG;
        [SerializeField] private CanvasGroup blink;

        public void SetAlpha(float value)
        {
            cG.alpha = value;
        }

        public void SetEff(Color col, float value)
        {
            foreach(var eff in effs)
                eff.color = Color.Lerp(eff.color, col, value);
        }

        public void Blink(float time, Action callback = null)
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(blink.DOFade(1f, time));
            seq.Append(blink.DOFade(0f, time));
            seq.OnComplete(() => callback?.Invoke());
        }
    }
}
