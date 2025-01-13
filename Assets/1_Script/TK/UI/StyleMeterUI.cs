using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class StyleMeterUI : MonoBehaviour
    {
        [SerializeField] private StyleMeter styleMeter;
        [SerializeField] private TextMeshProUGUI statMultiplierText;

        private void Start()
        {
            styleMeter.OnSuccessHitEvent += HandleMultiplierChanged;
            styleMeter.OnDamagedEvent += HandleMultiplierChanged;
        }

        private void HandleMultiplierChanged()
        {
            statMultiplierText.text = styleMeter.statMultiplier.ToString("0.00");
        }

        private void OnDestroy()
        {
            styleMeter.OnSuccessHitEvent -= HandleMultiplierChanged;
            styleMeter.OnDamagedEvent -= HandleMultiplierChanged;
        }
    }
}
