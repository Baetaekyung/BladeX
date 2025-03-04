using Swift_Blade.Enemy;
using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade
{
    public class SwordBossAnimatorController : BaseEnemyAnimationController
    {
        [SerializeField] private CameraShakeType cameraShakeType;

        public void CamShake()
        {
            CameraShakeManager.Instance.DoShake(cameraShakeType);    
        }
        
        
    }
}
