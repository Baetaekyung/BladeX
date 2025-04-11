
namespace Swift_Blade.UI
{
    public class ChallengeStageRemainTime
    {
        private float remainTime;
        
        public void SetRemainTime(float _remainTime)
        {
                        
            remainTime = _remainTime;
        }

        public float GetRemainTime()
        {
            return remainTime;
        }
        
        public void DecreaseRemainTime()
        {
            --remainTime;
        }
        
    }
}