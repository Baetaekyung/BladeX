using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Image _bossHealthFillAmount;
        [SerializeField] private EnemyHealth EnemyHealth;

        /// <param name="normalizedHealth"> input currentHealth / maxHealth</param>
        ///
        ///
        private void Start()
        {
            EnemyHealth.OnHitEvent += SetFillAmount;
        }

        private void OnDestroy()
        {
            EnemyHealth.OnHitEvent -= SetFillAmount;
        }

        private void SetFillAmount(float damageAmount)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateHealthFill(damageAmount));
        }

        private IEnumerator AnimateHealthFill(float targetFillAmount)
        {
            float startFillAmount = _bossHealthFillAmount.fillAmount;
            float duration = 0.2f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _bossHealthFillAmount.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsed / duration);
                yield return null;
            }
            
            _bossHealthFillAmount.fillAmount = targetFillAmount;
        }
    }
}
