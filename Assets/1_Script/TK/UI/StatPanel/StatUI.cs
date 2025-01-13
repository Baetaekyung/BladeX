using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Swift_Blade
{
    public class StatUI : MonoBehaviour
    {
        private RectTransform rectTrm => transform as RectTransform;
        [SerializeField] private CanvasGroup _cG;
        [SerializeField] private float _defaultXPos = -260f;
        [SerializeField] private Ease _showEase;
        [SerializeField] private Ease _hideEase;
        
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _agilityText;
        
        [SerializeField] private StatComponent _targetStat; //테스트용 Serialize
        private readonly StringBuilder _sb = new StringBuilder();

        //이거 호출해서 스텟 표시할 타겟 설정
        public void SetTargetStat(StatComponent statCompo) 
        {
            _targetStat = statCompo;
            
            UpdateStatUI();
        }

        //테스트용
         public void Update()
         {
             if (Input.GetKeyDown(KeyCode.G))
             {
                 ShowUIDelayUnShow(3f);
             }
         }

        public void ShowStatUI() //스텟 보여주기
        {
            UpdateStatUI();

            _cG.DOFade(1, 0.2f);
            rectTrm.DOAnchorPosX(0, 0.2f).SetEase(_showEase);
        }

        public void UnShowStatUI() //스텟 숨기기
        {
            _cG.DOFade(0, 0.2f);
            rectTrm.DOAnchorPosX(_defaultXPos, 0.2f).SetEase(_hideEase);
        }

        private void UpdateStatUI() //모든 스텟 세팅
        {
            SetStatUI(_targetStat.GetStatByType(StatType.HEALTH), _healthText);
            SetStatUI(_targetStat.GetStatByType(StatType.DAMAGE), _strengthText);
            SetStatUI(_targetStat.GetStatByType(StatType.AGILITY), _agilityText);
        }

        private void SetStatUI(StatSO stat, TextMeshProUGUI targetText, bool isPercent = false) //스텟 세팅
        {
            _sb.Clear();
            
            if(isPercent)
                _sb.Append(stat.displayName).Append(":  ").Append(stat.Value.ToString()).Append("%");
            else
                _sb.Append(stat.displayName).Append(":  ").Append(stat.Value.ToString());
            
            targetText.text = _sb.ToString();
        }

        public void ShowUIWithCallback(float delay, Action callback)
        {
            StartCoroutine(ShowUIWithCallbackRoutine(delay, callback));
        }
        
        public void ShowUIDelayUnShow(float delay)
        {
            StartCoroutine(ShowUIWithCallbackRoutine(delay, UnShowStatUI));
        }
        
        private IEnumerator ShowUIWithCallbackRoutine(float delay, Action callback)
        {
            ShowStatUI();
            yield return new WaitForSeconds(delay);

            callback?.Invoke();
        }
    }
}
