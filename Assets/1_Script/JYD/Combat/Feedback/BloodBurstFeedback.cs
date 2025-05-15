using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public class BloodBurstFeedback : Feedback
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO BloodParticle;
                        
        private void Start()
        {
            MonoGenericPool<BloodBurstParticle>.Initialize(BloodParticle);
        }

        public override void PlayFeedback()
        {
            MonoGenericPool<BloodBurstParticle>.Pop().transform.position = transform.position + new Vector3(0,1,0);
        }
        
        public override void ResetFeedback()
        {
            
        }
        
    }
}
