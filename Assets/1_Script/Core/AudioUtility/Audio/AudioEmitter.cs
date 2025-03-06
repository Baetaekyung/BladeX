using Swift_Blade.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Audio
{
    /// <summary>
    /// please only use AudioEmitter as runtime audio player.
    /// AudioEmitter can be preplaced though.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioEmitter : MonoBehaviour, IPoolable
    {
        public event Action OnEndCallback;
        private static readonly Dictionary<int, int> audioDictionary = new Dictionary<int, int>(20);

        [Header("preplaced")]
        [SerializeField] private AudioSO defaultAudioSO;
        [SerializeField] private bool prePlaced;

        [Header("General")]
        private AudioSource audioSource;
        private AudioSO currentAudioSO;

        private bool isInPool;                          //flag for checking if this is inside the pool
        private bool shouldDecraseCountOnDestroy;       //flag for OnDisable/OnDestroy
        private bool isInitialized;                     //flag for recyclePool Object

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            if (prePlaced)
            {
                bool flag = defaultAudioSO != null;
                Debug.Assert(flag, "emitter is preplaced but defaultAudioSO is null", this);
                Initialize(defaultAudioSO);
            }
        }
        void IPoolable.OnCreate()
        {
            isInPool = true;
        }
        void IPoolable.OnPopInitialize()
        {
            OnEndCallback       = default(Action);
            currentAudioSO      = default(AudioSO);
            isInPool            = default(bool);
            shouldDecraseCountOnDestroy   = default(bool);
            isInitialized       = default(bool);
        }
        public static void Dbg(AudioSO audioSO)
        {
            Debug.Log(audioDictionary[audioSO.clip.GetHashCode()]);
        }
        public static void Dbg2()
        {
            foreach (KeyValuePair<int, int> item in audioDictionary)
            {
                if (item.Value != 0)
                {
                    Debug.LogError("fuck" + item.Key);
                }
            }
        }
        private static bool IsAudioPlayable(AudioSO audioSO, bool autoIncrement = false)
        {
            if (!audioSO.enableMaxCount) return true;

            int hash = audioSO.clip.GetHashCode();
            audioDictionary.TryGetValue(hash, out int count);

            bool result = count < audioSO.maxCount;
            if (result && autoIncrement)
            {
                audioDictionary[hash] = ++count;
            }
            return result;
        }
        private static void DecreaseDictionaryInstance(AudioSO audioSO)
        {
            if (!audioSO.enableMaxCount) return;

            int hash = audioSO.clip.GetHashCode();
            int result = --audioDictionary[hash];
            //print(result);
            Debug.Assert(result >= 0, "yell at me ojy");
        }
        public void Play(bool destroyOnEnd = false)
        {
            if (isInPool) throw new Exception("audio emitter is already killed");

            bool flag = isInitialized && currentAudioSO != null;
            Debug.Assert(flag, "playing audio without initialization");

            if (audioSource.isPlaying)
                StopAudio();

            bool flag2 = IsAudioPlayable(currentAudioSO, true);
            if (!flag2)
            {
                Debug.LogWarning($"AudioInstance Reached Max {currentAudioSO.name}");
                return;
            }

            shouldDecraseCountOnDestroy = true;

            audioSource.Play();
            StartCoroutine(WaitUntilAudioEnd());
            
            IEnumerator WaitUntilAudioEnd()
            {
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
                OnEndCallback?.Invoke();
                DecreaseDictionaryInstance(currentAudioSO);
                shouldDecraseCountOnDestroy = false;

                if (destroyOnEnd)
                    KillAudio();
            }
        }
        public void PlayWithInit(AudioSO audioSO, bool killOnEnd = false)
        {
            if(audioSource.isPlaying)
                StopAudio();
            Initialize(audioSO);
            Play(killOnEnd);
        }
        /// <summary>
        /// plays without checking
        /// </summary>
        public void PlayOneShot()
        {
            if (isInPool) throw new Exception("audio emitter is already killed");

            //bool flag = IsAudioPlayable(currentAudioSO, true);
            //if (!flag)
            //{
            //    Debug.LogWarning($"AudioInstance Reached Max {currentAudioSO.name}");
            //    return;
            //}
            audioSource.PlayOneShot(currentAudioSO.clip, currentAudioSO.volume);
            //PlayOneShot(currentAudioSO);
        }
        //public void PlayOneShot(AudioSO audioSO)
        //{
        //    bool flag = IsCurrentAudioPlayable(currentAudioSO, true);
        //    if (!flag)
        //    {
        //        Debug.LogWarning($"AudioInstance Reached Max {currentAudioSO.name}");
        //        return;
        //    }
        //    else
        //    {
        //        audioSource.PlayOneShot(audioSO.clip);
        //    }
        //}
        public void PlayOneShotWithInit(AudioSO audioSO)
        {
            Initialize(audioSO);
            PlayOneShot();
        }
        /// <summary>
        /// note : stops coroutine
        /// </summary>
        public void StopAudio()
        {
            if (!audioSource.isPlaying) return;

            StopAllCoroutines();

            OnEndCallback?.Invoke();                    //should i call this?
            DecreaseDictionaryInstance(currentAudioSO);
            shouldDecraseCountOnDestroy = false;

            audioSource.Stop();
        }
        public void KillAudio()
        {
            if (isInPool)
            {
                Debug.LogWarning("audio emitter is already killed.");
                return;
            }

            StopAudio();
            MonoGenericPool<AudioEmitter>.Push(this);   //deactivate gameObject, auto cancel Coroutine.
            shouldDecraseCountOnDestroy = false;
            isInPool = true;
        }
        public void Initialize(AudioSO audioSO)
        {
            if (audioSource.isPlaying)
            {
                Debug.LogWarning($"n:{name}_initializing while playing audio");
                return;
            }

            isInitialized = true;

            currentAudioSO = audioSO;

            //global
            audioSource.clip = audioSO.clip;
            audioSource.outputAudioMixerGroup = audioSO.audioMixerGroup;

            //audioSource3D.loop
            //playoneshot
            if (prePlaced) return;

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

        private void OnDestroy()
        {
            if (!isInPool && shouldDecraseCountOnDestroy)
            {
                print("dontdestroy killing aud" + audioSource.clip.name);
                OnEndCallback?.Invoke();                    //should i call this?
                DecreaseDictionaryInstance(currentAudioSO);
            }

        }
    }
}
