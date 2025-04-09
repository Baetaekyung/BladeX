using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public class CameraFocusFeedback : Feedback
    {
        [field: SerializeField] public CameraFocusSO FocusData { get; set; }
        public override void PlayFeedback()
        {
            CameraFocusManager.Instance.StartFocus(FocusData);
        }

        public override void ResetFeedback()
        {

        }
    }
}
