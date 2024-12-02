using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade
{
    public class FeelingCheckScript : MonoBehaviour
    {
        [SerializeField] private HitStopSO _hitStopData;
        
        void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                CameraShakeManager.Instance.GenerateImpulse(shakeType: CameraShakeType.UpDown);
            }
            else if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                CameraShakeManager.Instance.GenerateImpulse(shakeType: CameraShakeType.LeftRight);
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                HitStopManager.Instance.HitStop(_hitStopData);
            }
        }
    }
}
