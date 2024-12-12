using System;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Image _bossHealthFillAmount;
        [SerializeField] private EnemyHealth EnemyHealth;

        /// <param name="normalizedHealth"> 현재 체력 / 최대 체력 넣기</param>
        private void Start()
        {
            EnemyHealth.OnHitEvent += SetFillAmount;
        }

        private void OnDestroy()
        {
            EnemyHealth.OnHitEvent -= SetFillAmount;
        }

        private void SetFillAmount(ActionData actionData)
        {
            _bossHealthFillAmount.fillAmount = actionData.healthPercent;
        }
    }
}
