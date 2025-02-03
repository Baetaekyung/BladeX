using Swift_Blade.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Audio
{
    public class AudioEmitter : MonoBehaviour, IPoolable
    {
        private AudioSource audioSource3D;
        public AudioClip CurrentClip { get; private set; }
        private void Awake()
        {
            audioSource3D = GetComponent<AudioSource>();
        }
        public void Play(AudioSO audioSO, bool destroyOnEnd = true)
        {
            Initialize(audioSO);
            audioSource3D.Play();
            if(destroyOnEnd)
                DestroyOnEnd();
        }
        private void DestroyOnEnd()
        {
            StartCoroutine(WaitUntilAudioEnd(() => { MonoGenericPool<AudioEmitter>.Push(this); }));
        }
        private IEnumerator WaitUntilAudioEnd(Action callBack)
        {
            Debug.Assert(callBack != null, "callback is null");
                print("st");
            while (audioSource3D.isPlaying)
            {
                print("waiting");
                yield return null;
            }
                print("end");
            callBack.Invoke();
        }
        private void Initialize(AudioSO audioSO)
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
        public void OnPopInitialize()
        {

        }
        private void OnDestroy()
        {
            print("dest");
            StopAllCoroutines();
        }
    }
}
