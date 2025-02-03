using Swift_Blade.Pool;
using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade.Audio
{
    public class AudioEmitter : MonoBehaviour, IPoolable
    {
        private AudioSource audioSource;
        public AudioSource GetAudioSource => audioSource;
        public AudioClip CurrentClip { get; private set; }
        private bool isAllowKill;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void OnPopInitialize()
        {
            isAllowKill = false;
        }
        public void Play(bool destroyOnEnd = false)
        {
            bool flag = audioSource.clip != null;
            Debug.Assert(flag, "playing audio without initialization");

            audioSource.Play();
            if (destroyOnEnd)
                DestroyOnEnd();
            isAllowKill = true;
        }
        public void PlayWithInit(AudioSO audioSO, bool destroyOnEnd = false)
        {
            Initialize(audioSO);
            Play(destroyOnEnd);
        }
        public void PlayOneShot(AudioSO audioSO)
        {
            audioSource.PlayOneShot(audioSO.clip);
            isAllowKill = true;
        }
        public void PlayOneShotWithInit(AudioSO audioSO)
        {
            Initialize(audioSO);
            PlayOneShot(audioSO);
        }
        public void StopAudio()
        {
            audioSource.Stop();
        }
        public void KillAudio()
        {
            //StopAllCoroutines();
            StopAudio();
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
            print("st");
            while (audioSource.isPlaying)
            {
                print("waiting");
                yield return null;
            }
            print("end");
            callBack.Invoke();
        }
        public void Initialize(AudioSO audioSO)
        {
            //global

            CurrentClip = audioSO.clip;

            audioSource.clip = audioSO.clip;
            audioSource.outputAudioMixerGroup = audioSO.audioMixerGroup;
            //audioSource3D.loop
            //playoneshot
            audioSource.priority = audioSO.priority;
            audioSource.volume = audioSO.volume;
            audioSource.pitch = audioSO.pitch;
            audioSource.panStereo = audioSO.streoPan;
            audioSource.spatialBlend = audioSO.GetSpatialBlend;
            audioSource.reverbZoneMix = audioSO.reverbZoneMix;

            //3DSOUND SETTINGS
            audioSource.dopplerLevel = audioSO.dopplerLevel;
            audioSource.spread = audioSO.spread;
            audioSource.rolloffMode = audioSO.audioRolloffMode;
            audioSource.minDistance = audioSO.minDistance;
            audioSource.maxDistance = audioSO.maxDistance;
            if (audioSO.audioRolloffMode == AudioRolloffMode.Custom)
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioSO.curve);
        }

    }
}
