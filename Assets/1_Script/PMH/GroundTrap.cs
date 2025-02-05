using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class GroundTrap : MonoBehaviour
    {
        private bool isActive = false;

        [SerializeField] private Transform trapSpears;
        [SerializeField] private float delay = 1;
        private float timer;

        private void Start()
        {
            trapSpears.localPosition = new Vector3(0, GetYValue());
            //StartCoroutine(ActiveCoroutine());
        }
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > delay)
            {
                timer = 0;
                isActive = !isActive;
                float targetY = GetYValue();
                trapSpears.DOMoveY(targetY, 0.5f);
            }
        }
        private float GetYValue()
        {
            const float activeYValue = 0;
            const float deactiveYValue = -2;
            return isActive ? activeYValue : deactiveYValue;
        }
        private void OnTriggerStay(Collider other)
        {
            if (!isActive) return;
            if (other.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(new ActionData { damageAmount = 1, attackType = AttackType.Melee });
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
