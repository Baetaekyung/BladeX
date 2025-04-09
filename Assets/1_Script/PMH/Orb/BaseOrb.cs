using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class BaseOrb : MonoBehaviour, IInteractable
    {
        [SerializeField] private float startFadeScale;
        [SerializeField] private float collectFadeEndDuration;

        [SerializeField] private Material[] colors;
        private MeshRenderer itemRenderer;

        private bool isCollected;
        protected static int a = 23;

        private void Awake()
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
            if (isCollected)
            {
                return;
            }

            isCollected = true;

            HandleInteract(CreateDefaultCallback());
        }
        protected abstract TweenCallback CreateDefaultCallback();
        protected void HandleInteract(TweenCallback onComplete)
        {
            bool isCallbackNull = onComplete == null;
            Debug.Assert(!isCallbackNull, "default callback is null");
            Collect(onComplete);
        }
        protected void Collect(TweenCallback onComplete)
        {
            transform.DOScale(0.01f, collectFadeEndDuration)
                .SetDelay(0.1f)
                .SetEase(Ease.OutExpo)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                .OnComplete(onComplete);
        }
    }
}
