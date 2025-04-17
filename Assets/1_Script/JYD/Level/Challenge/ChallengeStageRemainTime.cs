using UnityEngine;

namespace Swift_Blade.UI
{
    public class ChallengeStageRemainTime
    {
        private int remainTime;
        
        public void SetRemainTime(float _remainTime)
        {
            remainTime = Mathf.RoundToInt(_remainTime);
        }

        public int GetRemainTime()
        {
            return remainTime;
        }
        
        public void DecreaseRemainTime()
        {
            --remainTime;
        }
        
    }
}