using DG.Tweening;
using Swift_Blade.Combat.Health;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade.Level.Obstacle
{
    public class GroundTrap : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform trapSpears;
        [SerializeField, Range(0.6f, 2)] private float delay = 1;// note this must be longer than tween duration
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Active/Inactive")]
        [SerializeField] private AnimationCurve easeCurbeActive;
        [SerializeField] private AnimationCurve easeCurveInactive;

        [SerializeField] private UnityEvent UE_OnActive;

        [SerializeField] private Color32 colorActive;
        private Color32 colorInactive;


        private bool isActive;
        private float timer;
        private void Awake()
        {
            colorInactive = meshRenderer.material.color;
            trapSpears.localPosition = new Vector3(0, GetYValue());

        }
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                timer = 0;
                isActive = !isActive;

                AnimationCurve curve = isActive ? easeCurbeActive : easeCurveInactive;
                Color32 color = isActive ? colorActive : colorInactive;

                const float duration = 0.5f;
                float targetY = GetYValue();
                trapSpears.DOLocalMoveY(targetY, duration).SetEase(curve);
                meshRenderer.material.DOColor(color, duration).SetEase(curve);
                UE_OnActive?.Invoke();
            }
        }
        private float GetYValue()
        {
            const float yActive = 0;
            const float yInactive = -2.5f;
            return isActive ? yActive : yInactive;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!isActive) return;
            if (other.TryGetComponent(out BaseEntityHealth health))
            {
                health.TakeDamage(new ActionData { damageAmount = 1, stun = health is PlayerHealth });
            }
        }
        //IEnumerator ActiveCoroutine()
        //{
        //    while (true)
        //    {
        //        if (isActive)
        //        {
        //            trapSpears.DOMoveY(transform.localPosition.y - 2, 0.5f);
        //            isActive = false;
        //        }
        //        else
        //        {
        //            trapSpears.DOMoveY(transform.localPosition.y, 0.5f);
        //            isActive = true;
        //        }
        //
        //        yield return new WaitForSeconds(delay);
        //    }
        //}
    }
}
