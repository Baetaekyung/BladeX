using System;
using UnityEngine;
using TMPro;

namespace Swift_Blade.UI
{
    
    public class ChallengeStageUIView : MonoBehaviour
    {
        public TextMeshProUGUI remainText;
        
        private void Start()
        {
            PopupManager.Instance.LogMessage("������ ��Ƴ��ƶ�");
        }
        
        public void SetText(int _remainCount)
        {
            
            remainText.SetText(_remainCount != 0 ?_remainCount.ToString() : "");
        }
        
    }
}
