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
            PopupManager.Instance.LogMessage("끝까지 살아남아라");
        }
        
        public void SetText(int _remainCount)
        {
            
            remainText.SetText(_remainCount != 0 ?_remainCount.ToString() : "");
        }
        
    }
}
