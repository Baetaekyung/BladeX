using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    [Serializable]
    public struct TargetStatTypes
    {
        public List<StatType> targetStats;
    }
    
    [CreateAssetMenu(fileName = "StyleMeter", menuName = "SO/StyleMeter")]
    public class StyleMeter : ScriptableObject
    {
        public event Action OnSuccessHitEvent;
        public event Action OnDamagedEvent;

        [SerializeField] private float maxMultiplier;
        
        public float appliedMultiplier;
        private const float StatMultiplier = 1f;
        private float _addedMultiplier = 0f; //���� ������ �÷��ִ� multiplier 

        public List<TargetStatTypes> TargetStatTypes = new List<TargetStatTypes>();
        public PlayerStatCompo PlayerStat;

        private void OnEnable()
        {
            appliedMultiplier = 1f;
            _addedMultiplier = 0f;
        }

        public void SuccessHit()
        {
            IncreaseMultiplier(0.01f); //Stat���� ������ �޾Ƽ� Increase��Ű��
            
            OnSuccessHitEvent?.Invoke();
        }

        public void TakeDamage()
        {
            DecreaseMultiplier(0.01f); //Stat���� ���ҷ� �޾Ƽ� Decrease��Ű��

            OnDamagedEvent?.Invoke();
        }

        private void IncreaseMultiplier(float increaseAmount)
        {
            _addedMultiplier += increaseAmount;

            foreach (var targetStatType in TargetStatTypes[Mathf.FloorToInt(appliedMultiplier - 1f)].targetStats)
            {
                Debug.Log($"{targetStatType.ToString()} : {PlayerStat.GetStatByType(targetStatType).Value}");
            }
            
            ApplyMultiplier();
        }

        private void DecreaseMultiplier(float decreaseAmount)
        {
            _addedMultiplier -= decreaseAmount;
            
            foreach (var targetStatType in TargetStatTypes[Mathf.FloorToInt(appliedMultiplier - 1f)].targetStats)
            {
                Debug.Log($"{targetStatType.ToString()} : {PlayerStat.GetStatByType(targetStatType).Value}");
            }
            
            ApplyMultiplier();
        }
        
        private void ApplyMultiplier()
        {
            _addedMultiplier = Mathf.Clamp(_addedMultiplier, 0f, maxMultiplier);
            appliedMultiplier = Mathf.Clamp(StatMultiplier + _addedMultiplier, 1f, maxMultiplier + 1f);
        }
    }
}
