using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Swift_Blade.Combat.Health;

namespace Swift_Blade.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Image _bossHealthFillAmount;
        [SerializeField] private BaseEnemyHealth swordBaseBossHealth;
        
        /// <param name="normalizedHealth"> 현재 체력 / 최대 체력 넣기</param>
        private void Start()
        {
            swordBaseBossHealth.OnChangeHealthEvent += SetFillAmount;
        }

        private void OnDestroy()
        {
            swordBaseBossHealth.OnChangeHealthEvent -= SetFillAmount;
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
