using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Swift_Blade.Feeling
{
    
    public class PostProcessManager : MonoSingleton<PostProcessManager>
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private VolumeProfile _defaultProfile;

        public void DoPostProcessing(VolumeProfile profile ,float time)
        {
            StartCoroutine(PostProcessingRoutine(profile, time));
        }

        private IEnumerator PostProcessingRoutine(VolumeProfile profile, float time)
        {
            _volume.profile = profile;

            yield return new WaitForSeconds(time);

            _volume.profile = _defaultProfile;
        }
    }
}
