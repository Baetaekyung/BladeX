using System;
using System.Collections.Generic;
using UnityEngine;


namespace Swift_Blade
{
    public enum StyleMeterState 
    {
        First, //이름 뭐라 해야할 지 모르겠음;;
        Second,
        Third
    }
    
    [Serializable]
    public struct TargetStatTypes
    {
        public List<StatType> targetStats;
    }
    
    [CreateAssetMenu(fileName = "StyleMeter", menuName = "SO/StyleMeter")]
    public class StyleMeter : ScriptableObject
    {
        // public event Action OnSuccessHitEvent;
        // public event Action OnDamagedEvent;
        //
        // [SerializeField] private float maxMultiplier;
        //
        // [SerializeField] private float maxTimeModifierValue = 1.5f;
        // [SerializeField] private float minTimeModifierValue = -0.1f;
        // private float? _timeModifier = -0.1f;
        //
        // public float appliedMultiplier;
        // private const float StatMultiplier = 1f;
        // private float _addedMultiplier = 0f; //공격 성공시 올려주는 multiplier 
        // private StyleMeterState _styleMeterState = StyleMeterState.First;
        //
        // [SerializeField] private List<TargetStatTypes> targetStatTypes = new();
        // public PlayerStatCompo PlayerStat;
        //
        // public float GetModifierValue => _timeModifier ?? -0.1f;
        //
        // //초기화인데 초기화 안할거라고 하는..
        // public void Init()
        // {
        //     appliedMultiplier = 1f;
        //     _addedMultiplier = 0f;
        //     _timeModifier = -0.1f;
        // }
        //
        // public void SuccessHit()
        // {
        //     const float initialValue = 0.1f;
        //     float amount = initialValue + PlayerStat.GetStat(StatType.STYLE_METER_INCREASE_INCREMENT).Value;
        //     IncreaseMultiplier(amount); //Stat에서 증가량 받아서 Increase시키기
        //     
        //     OnSuccessHitEvent?.Invoke();
        // }
        //
        // private void ChangeTimeModifier(float value)
        // {
        //     _timeModifier = Mathf.Lerp(minTimeModifierValue, maxTimeModifierValue,
        //         value / maxMultiplier);
        // }
        //
        // public void TakeDamage()
        // {
        //     const float initialValue = 0.1f;
        //     float amount = initialValue + PlayerStat.GetStat(StatType.STYLE_METER_DECREASE_DECREMENT).Value;
        //     DecreaseMultiplier(amount); //Stat에서 감소량 받아서 Decrease시키기
        //     
        //     OnDamagedEvent?.Invoke();
        // }
        //
        // public List<StatType> GetTargetStats()
        // {
        //     switch (_styleMeterState)
        //     {
        //         case StyleMeterState.First:
        //             return targetStatTypes[0].targetStats;
        //         case StyleMeterState.Second:
        //             return targetStatTypes[1].targetStats;
        //         case StyleMeterState.Third:
        //             return targetStatTypes[2].targetStats;
        //     }
        //
        //     return default;
        // }
        //
        // private void IncreaseMultiplier(float increaseAmount)
        // {
        //     _addedMultiplier += increaseAmount;
        //     ApplyMultiplier();
        // }
        //
        // private void DecreaseMultiplier(float decreaseAmount)
        // {
        //     _addedMultiplier -= decreaseAmount;
        //     ApplyMultiplier();
        // }
        //
        // private void ApplyMultiplier()
        // {
        //     _addedMultiplier = Mathf.Clamp(_addedMultiplier, 0f, maxMultiplier);
        //     appliedMultiplier = Mathf.Clamp(StatMultiplier + _addedMultiplier, 1f, maxMultiplier + 1f);
        //     
        //     ChangeTimeModifier(_addedMultiplier);
        //
        //     _styleMeterState = _addedMultiplier switch
        //     {
        //         < 1f => StyleMeterState.First,
        //         >= 1f and < 2f => StyleMeterState.Second,
        //         >= 2f => StyleMeterState.Third,
        //         _ => _styleMeterState
        //     };
        // }
    }
}
