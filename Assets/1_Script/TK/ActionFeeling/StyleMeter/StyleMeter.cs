using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "StyleMeter", menuName = "SO/StyleMeter")]
    public class StyleMeter : ScriptableObject
    {
        public event Action OnSuccessHitEvent;
        public event Action OnDamagedEvent;

        public float statMultiplier = 1f;

        private void OnEnable()
        {
            statMultiplier = 1f;
        }

        public void SuccessHit()
        {
            statMultiplier += 0.01f;

            OnSuccessHitEvent?.Invoke();
        }

        public void MeterInitialize()
        {
            statMultiplier = 1f;

            OnDamagedEvent?.Invoke();
        }
    }
}
