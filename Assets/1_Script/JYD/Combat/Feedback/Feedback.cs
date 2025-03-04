using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void PlayFeedback();
        public abstract void ResetFeedback();
        
    }
}
