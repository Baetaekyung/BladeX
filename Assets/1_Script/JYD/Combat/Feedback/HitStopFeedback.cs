using UnityEngine;
using Swift_Blade.Feeling;

namespace Swift_Blade.Combat.Feedback
{
    public class HitStopFeedback : Feedback
    {
        [field: SerializeField] public HitStopSO HitStopData { get; set; }



        public override void PlayFeedback()
        {

            HitStopManager.Instance.StartHitStop(HitStopData);
        }

        public override void ResetFeedback()
        {

        }


    }
}
