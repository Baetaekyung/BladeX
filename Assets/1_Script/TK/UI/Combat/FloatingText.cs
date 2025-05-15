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

            //�������� ǥ���ϴ� ���̸� �������翡 ���� ũ�� ���� �ֱ�            
            if (float.TryParse(message, out float value))
            {
                Vector3 localScale = transform.localScale;

                //�������� 100�� ������ �ִ� ũ��
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
                .SetEase(Ease.Flash) // ũ�Ⱑ Ŀ����
                .Join(transform.DOMoveY(
                    transform.position.y + randomFloatSpeed, floatingTime))
                .SetEase(Ease.OutQuint) // ���ÿ� ���� �ö�
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            seq.Append(floatingText.DOColor(Color.clear, fadeTime)) // ������ Fade�ϰ�
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
