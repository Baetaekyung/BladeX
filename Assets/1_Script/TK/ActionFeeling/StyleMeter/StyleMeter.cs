using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public enum StyleMeterScore : short
    {
        NONE = 0,
        D = 1,
        C = 2,
        B = 3,
        A = 4,
        S = 5,
        Swift = 6
    }
    
    public class StyleMeter : MonoSingleton<StyleMeter>
    {
        private readonly int _changeMeterUPAnimHash = Animator.StringToHash("ChangeMeterUP");
        private readonly int _changeMeterDOWNAnimHash = Animator.StringToHash("ChangeMeterDOWN");

        public Action<StyleMeterScore> OnStyleMeterUpEvent;
        public Action<StyleMeterScore> OnStyleMeterDownEvent;
        
        [Header("About visual")]
        private Animator _animator;
        [SerializeField] private Image _swiftIcon;
        
        [Header("StyleMeter Score")]
        private StyleMeterScore _currentMeterScore = StyleMeterScore.NONE;

        [Header("Timer")]
        [SerializeField] private float _downMeterScoreTime = 6f; //스타일 미터 유지 시간
        private float _currentTime = 0f;
        private float _meterUpPercent = 0f;
        private int _currentMeterRank = 0; //미터 스코어 정수화

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            
            float normalizedValue = (float)_currentMeterScore / 6;
            _swiftIcon.fillAmount = normalizedValue;
        }

        private void Update()
        {
            if (_downMeterScoreTime <= _currentTime)
                DowngradeMeterRank();
            else
                _currentTime += Time.deltaTime;
        }

        public void RaiseMeterPercent(float raisePercent)
        {
            _meterUpPercent += raisePercent;
            
            if (_meterUpPercent >= 100f)
                UpgradeMeterRank();
        }
        
        public void UpgradeMeterRank()
        {
            if (_currentMeterScore == StyleMeterScore.Swift) return;

            _meterUpPercent = 0f;
            _currentTime = 0f; //다운그레이드 초기화
            
            _currentMeterRank++;
            _currentMeterScore = (StyleMeterScore)_currentMeterRank;
            
            OnStyleMeterUpEvent?.Invoke(_currentMeterScore);

            float normalizedValue = (float)_currentMeterScore / 6;
            _swiftIcon.fillAmount = normalizedValue;
            
            _animator.SetTrigger(_changeMeterUPAnimHash);
        }

        public void DowngradeMeterRank()
        {
            _currentTime = 0f;
            if (_currentMeterScore == StyleMeterScore.NONE) return;

            _currentMeterRank--;
            _currentMeterScore = (StyleMeterScore)_currentMeterRank;
            
            OnStyleMeterDownEvent?.Invoke(_currentMeterScore);
            
            _animator.SetTrigger(_changeMeterDOWNAnimHash);
            
            float normalizedValue = (float)_currentMeterScore / 6;
            _swiftIcon.fillAmount = normalizedValue;
        }
    }
}
