using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class BaseOrb : MonoBehaviour, IInteractable
    {
        [SerializeField] protected float startFadeScale;
        [SerializeField] protected float collectFadeEndDuration;

        [SerializeField] private Material[] colors;
        private MeshRenderer itemRenderer;

        protected virtual bool CanInteract => !isCollected;
        private bool isCollected;

        private Tween interactTween;

        protected virtual void Awake()
        {
            itemRenderer = GetComponent<MeshRenderer>();
            const float START_FADE_DURATION = 0.75f;
            transform.DOScale(startFadeScale, START_FADE_DURATION)
                .SetDelay(0.1f)
                .SetEase(Ease.OutElastic)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        public void SetColor(ColorType color)
        {
            itemRenderer.material = colors[(int)color];
        }
        void IInteractable.Interact()
        {
            if (!CanInteract)
            {
                return;
            }

            Interact();
        }
        protected virtual Tween InteractTween()
        {
            return transform.DOScale(0.01f, collectFadeEndDuration)
                .SetDelay(0.1f)
                .SetEase(Ease.OutExpo)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        protected virtual TweenCallback CollectTweenCallback() => null;
        protected virtual void Interact()
        {
            isCollected = true;

            if (interactTween != null)
            {
                interactTween.Kill();
            }

            interactTween = InteractTween();

            TweenCallback onComplete = CollectTweenCallback();
            bool isCallbackNull = onComplete == null;
            if (!isCallbackNull)
            {
                interactTween.OnComplete(onComplete);
            }
        }
    }
}