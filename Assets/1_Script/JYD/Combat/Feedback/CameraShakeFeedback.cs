using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public class CameraShakeFeedback : Feedback
    {
        [field: SerializeField] public CameraShakeType ShakeType { get; set; }

        public override void PlayFeedback()
        {
            CameraShakeManager.Instance.DoShake(ShakeType);
        }

        public override void ResetFeedback()
        {

        }
    }
}
