using DG.Tweening;
using Swift_Blade.Pool;
using System;
using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class FloatingText : MonoBehaviour, IPoolable
    {
        public event Action OnComplete;

        [SerializeField] private TextMeshPro floatingText;
        [SerializeField] private float floatSpeed;
        [SerializeField] private float additiveScale;
        [SerializeField] private float floatingTime;
        [SerializeField] private float fadeTime = 1.2f;

        private Camera cam;
        public Sequence seq;

        public void SetText(string message)
        {
            floatingText.text = message;

            //데미지를 표현하는 것이면 데미지양에 따라 크기 변동 주기            
            if (float.TryParse(message, out float value))
            {
                Vector3 localScale = transform.localScale;

                //데미지가 100이 넘으면 최대 크기
                float additiveScale = Mathf.Lerp(0, 2f, (value / 100));

                transform.localScale = new Vector3(
                    localScale.x + additiveScale,
                    localScale.y + additiveScale,
                    localScale.z + additiveScale);
            }
        }

        public void SetText(string message, Color color)
        {
            floatingText.color = color;

            SetText(message);
        }

        private void LateUpdate()
        {
            if (cam == null) return;

            Vector3 direction = transform.position - cam.transform.position;

            transform.rotation = Quaternion.LookRotation(direction);
        }

        public void Animation()
        {
            seq = DOTween.Sequence();

            float randomFloatSpeed = UnityEngine.Random.Range(floatSpeed * 0.7f, floatSpeed);

            seq.Append(transform.DOScale(transform.localScale.x + additiveScale, floatingTime))
                .SetEase(Ease.Flash) // 크기가 커짐과
                .Join(transform.DOMoveY(
                    transform.position.y + randomFloatSpeed, floatingTime))
                .SetEase(Ease.OutQuint) // 동시에 위로 올라감
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            seq.Append(floatingText.DOColor(Color.clear, fadeTime)) // 끝나면 Fade하고
                    .OnComplete(() => OnComplete?.Invoke())
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        void IPoolable.OnPush()
        {
            if (seq != null)
                seq.Kill();

            OnComplete = null;

            cam = null;
            transform.localScale = Vector3.one;
            floatingText.color = Color.white;
            floatingText.text = string.Empty;
        }

        public void OnPop()
        {
            cam = Player.Instance.GetPlayerCamera.GetPlayerCamera;
        }
    }
}
