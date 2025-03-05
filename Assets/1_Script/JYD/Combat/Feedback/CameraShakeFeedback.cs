using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public class CameraShakeFeedback : Feedback
    {
        public CameraShakeType shakeType;
        
        public override void PlayFeedback()
        {
            CameraShakeManager.Instance.DoShake(shakeType);
        }

        public override void ResetFeedback()
        {
            
        }
    }
}
