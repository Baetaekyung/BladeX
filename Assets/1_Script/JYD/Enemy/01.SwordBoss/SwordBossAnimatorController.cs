using Swift_Blade.Feeling;
using UnityEngine;

namespace Swift_Blade.Enemy.Sword
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
