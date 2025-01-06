using System.Collections.Generic;
using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Combat.Feedbck
{
    public class HitStopFeedback : Feedback
    {
        public HitStopSO hitStopData;
        
        
        public override void PlayFeedback()
        {
               
            HitStopManager.Instance.DoHitStop(hitStopData);
        }

        public override void ResetFeedback()
        {
            
        }
    }
}
