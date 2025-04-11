using UnityEngine;
using TMPro;

namespace Swift_Blade.UI
{
    
    public class ChallengeStageUIView : MonoBehaviour
    {
        public TextMeshProUGUI remainText;
        
        public void SetText(float _remainCount)
        {
            remainText.SetText(_remainCount.ToString());
        }
        
    }
}
