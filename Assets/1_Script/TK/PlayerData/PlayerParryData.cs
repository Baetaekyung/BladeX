using Swift_Blade.Feeling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class PlayerParryData : MonoBehaviour
    {
        [FormerlySerializedAs("_hitStopData")] public HitStopSO hitStopData;
        [FormerlySerializedAs("_cameraFocusData")] public CameraFocusSO cameraFocusData;
        [FormerlySerializedAs("_cameraShakeData")] public CameraShakeSO cameraShakeData;
        [FormerlySerializedAs("_parryPostProcessing")] public VolumeProfile parryPostProcessing;
        public GameObject parryLight;
    }
}
