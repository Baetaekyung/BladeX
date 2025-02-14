using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Swift_Blade
{
    public class StyleMeterUI : MonoBehaviour
    {
        [SerializeField] private StyleMeter styleMeter;
        [SerializeField] private TextMeshProUGUI statMultiplierText;
        
        [Header("Style meter effect")]
        [SerializeField] private Image styleMeterEffect;
        [SerializeField] private float maxEffectShakeIntensity = 3.5f;
        [SerializeField] private float effectShakeIntensity = 0f;
        [SerializeField] private bool isShake = false;
        
        private readonly float _maxEffectSize = 2f;
        private float _currentEffectSize;
        
        private void Start()
        {
            styleMeter.OnSuccessHitEvent += HandleMultiplierChanged;
            styleMeter.OnDamagedEvent += HandleMultiplierChanged;
            
             StyleMeterEffectChanged();
        }

        private void Update()
        {
            HandleEffectShake();
        }

        private void HandleEffectShake()
        {
            if (isShake && styleMeterEffect.IsActive())
            {
                Vector2 randomPosition = Random.insideUnitCircle;
                Vector2 forcedPosition = randomPosition * effectShakeIntensity;

                float randomRotation = Random.Range(0, 0.1f);
                float forcedRotation = randomRotation * effectShakeIntensity;
                
                styleMeterEffect.rectTransform.anchoredPosition = forcedPosition;
                styleMeterEffect.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, forcedRotation));
            }
        }

        private void HandleMultiplierChanged()
        {
            statMultiplierText.text = styleMeter.appliedMultiplier.ToString("0.00");
            StyleMeterEffectChanged();
        }

        private void StyleMeterEffectChanged()
        {
            _currentEffectSize = styleMeter.addedMultiplier / _maxEffectSize;
            _currentEffectSize = Mathf.Clamp(_currentEffectSize, 0f, 1f);
            
            effectShakeIntensity = Mathf.Lerp(0, maxEffectShakeIntensity, _currentEffectSize);
            
            styleMeterEffect.color = new Color(styleMeterEffect.color.r,
                styleMeterEffect.color.g, styleMeterEffect.color.b,
                Mathf.Lerp(0f, 1f, _currentEffectSize));
        }

        private void OnDestroy()
        {
            styleMeter.OnSuccessHitEvent -= HandleMultiplierChanged;
            styleMeter.OnDamagedEvent -= HandleMultiplierChanged;
        }
    }
}
