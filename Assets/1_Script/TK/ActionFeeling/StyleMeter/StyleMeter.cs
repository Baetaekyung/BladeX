using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public enum MeterState
    {
        NONE,
        B,
        S
    }
    
    [CreateAssetMenu(fileName = "StyleMeter", menuName = "SO/StyleMeter")]
    public class StyleMeter : ScriptableObject
    {
        public event Action OnSuccessHitEvent;
        public event Action OnDamagedEvent;

        public float appliedMultiplier;
        public float statMultiplier = 1f;
        public float addedMultiplier = 0f;

        public MeterState CurrentState = MeterState.NONE;
        
        public void SuccessHit()
        {
            addedMultiplier += 0.01f;
            appliedMultiplier = statMultiplier + addedMultiplier;

            OnSuccessHitEvent?.Invoke();
        }

        public void MeterInitialize()
        {
            addedMultiplier = 0f;
            appliedMultiplier = statMultiplier + addedMultiplier;

            OnDamagedEvent?.Invoke();
        }
    }
}
