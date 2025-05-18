using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] private Image gauge;

        private float _currentGauge;

        private void OnEnable()
        {
            _currentGauge = Player.level.Experience / Player.level.NextExperience;
            gauge.fillAmount = _currentGauge;
        }
        
        private void Update()
        {
            _currentGauge = Mathf.Lerp(_currentGauge, Player.level.Experience / Player.level.NextExperience, Time.deltaTime * 4f);
            gauge.fillAmount = _currentGauge;
        }
    }
}
