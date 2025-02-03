using Swift_Blade.Pool;
using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade.Audio
{
    public class AudioEmitter : MonoBehaviour, IPoolable
    {
        private AudioSource audioSource3D;
        public AudioClip CurrentClip { get; private set; }
        private bool isAllowKill;
        private void Awake()
        {
            audioSource3D = GetComponent<AudioSource>();
        }
        public void OnPopInitialize()
        {
            isAllowKill = false;
        }
        public void Play(bool destroyOnEnd = true)
        {
            audioSource3D.Play();
            if (destroyOnEnd)
                DestroyOnEnd();
            isAllowKill = true;
        }
        public void Play(AudioSO audioSO, bool destroyOnEnd = true)
        {
            Initialize(audioSO);
            Play(destroyOnEnd);
        }
        public void PlayOneShot(AudioSO audioSO)
        {
            audioSource3D.PlayOneShot(audioSO.clip);
            isAllowKill = true;
        }
        public void StopAudio()
        {
            audioSource3D.Stop();
        }
        public void KillAudio()
        {
            StopAllCoroutines();
            Debug.Assert(isAllowKill, "killing unkillable emitter instacne");
            if(isAllowKill)
                MonoGenericPool<AudioEmitter>.Push(this);
        }

        private void DestroyOnEnd()
        {
            StartCoroutine(WaitUntilAudioEnd(KillAudio));
        }
        private IEnumerator WaitUntilAudioEnd(Action callBack)
        {
            Debug.Assert(callBack != null, "callback is null");
            //print("st");
            while (audioSource3D.isPlaying)
            {
                //print("waiting");
                yield return null;
            }
            //print("end");
            callBack.Invoke();
        }
        public void Initialize(AudioSO audioSO)
        {
            CurrentClip = audioSO.clip;

            audioSource3D.clip = audioSO.clip;
            audioSource3D.outputAudioMixerGroup = audioSO.audioMixerGroup;
            //audioSource3D.loop
            //playoneshot
            audioSource3D.priority = audioSO.priority;
            audioSource3D.volume = audioSO.volume;
            audioSource3D.pitch = audioSO.pitch;
            audioSource3D.panStereo = audioSO.streoPan;
            audioSource3D.spatialBlend = audioSO.GetSpatialBlend;
            audioSource3D.reverbZoneMix = audioSO.reverbZoneMix;

            //3DSOUND SETTINGS
            audioSource3D.dopplerLevel = audioSO.dopplerLevel;
            audioSource3D.spread = audioSO.spread;
            audioSource3D.rolloffMode = audioSO.audioRolloffMode;
            audioSource3D.minDistance = audioSO.minDistance;
            audioSource3D.maxDistance = audioSO.maxDistance;
            if (audioSO.audioRolloffMode == AudioRolloffMode.Custom)
                audioSource3D.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioSO.curve);
        }

    }
}
