using Swift_Blade.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Audio
{
    public class AudioEmitter : MonoBehaviour, IPoolable
    {
        private static readonly Dictionary<int, int> audioDictionary = new Dictionary<int, int>(20);

        public event Action OnEndCallback;

        [Header("Preplace")]
        [SerializeField] private BaseAudioSO defaultAudioSO;
        [SerializeField] private bool overrideVolume;
        private bool isPrePlaced = true;

        [Header("General")]
        private AudioSource audioSource;
        private AudioSO currentAudioSO;

        private bool IsInitialized => currentAudioSO != null;
        private bool isInPool;                          //flag for checking if this is inside the pool
        private bool shouldDecraseCountOnDestroy;       //flag for OnDisable/OnDestroy

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        private void Start()    //black magick, this is delayed when pooled.
        {
            if (isPrePlaced)      //not initialized until IPoolable.OnCreate
            {
                bool flag = defaultAudioSO != null;
                Debug.Assert(flag, "emitter is preplaced but defaultAudioSO is null", this);
                Initialize(defaultAudioSO.GetAudio());
            }
        }
        void IPoolable.OnCreate()
        {
            isInPool = true;
            isPrePlaced = false;
        }
        void IPoolable.OnPop()
        {
            isInPool = false;
            shouldDecraseCountOnDestroy = false;
        }
        void IPoolable.OnPush()
        {
            isInPool = true;
            shouldDecraseCountOnDestroy = false;

            OnEndCallback = null;
            currentAudioSO = null;
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
                    Debug.LogError("err " + item.Key);
                }
            }
        }
        private static bool IsAudioPlayable(AudioSO audioSO, bool autoIncrement = false)
        {
            return true;
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
            return;
            if (!audioSO.enableMaxCount) return;

            int hash = audioSO.clip.GetHashCode();
            int result = --audioDictionary[hash];
            Debug.Assert(result >= 0, "yell at me ojy");
        }
        public void Play(bool destroyOnEnd = false)
        {
            if (isInPool) throw new InvalidOperationException("audio emitter is already killed");
            if (!IsInitialized) throw new InvalidOperationException("playing without init");
                
            if (audioSource.isPlaying)
                StopAudio();

            bool flagPlayable = IsAudioPlayable(currentAudioSO, true);
            if (!flagPlayable)
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

                OnAudioStop();

                if (destroyOnEnd)
                    KillAudio();
            }
        }
        public void PlayWithInitDefaultAudio(bool killOnEnd = false)
        {
            if (audioSource.isPlaying)
                StopAudio();
            Initialize(defaultAudioSO.GetAudio());
            Play(killOnEnd);
        }
        public void PlayWithInit(AudioSO audioSO, bool killOnEnd = false)
        {
            if (audioSource.isPlaying) //can't initialize without stopping audio
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
            if (!IsInitialized) throw new Exception("playing without init");

            audioSource.PlayOneShot(currentAudioSO.clip, currentAudioSO.Volume);
        }
        public void PlayOneShotWithInitDefaultAudio(bool killOnEnd = false)
        {
            if (audioSource.isPlaying)
                StopAudio();
            Initialize(defaultAudioSO.GetAudio());
            PlayOneShot();
        }
        public void PlayOneShotWithInit(AudioSO audioSO)
        {
            if (audioSource.isPlaying) //can't initialize without stopping audio
                StopAudio();
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

            OnAudioStop();

            audioSource.Stop();
        }
        /// <summary>
        /// return audio to pool
        /// </summary>
        /// <exception cref="InvalidOperationException">instance is already in pool</exception>
        public void KillAudio()
        {
            if (isInPool) throw new InvalidOperationException("audio emitter is already killed");

            StopAudio();
            MonoGenericPool<AudioEmitter>.Push(this);   //deactivate gameObject, auto cancel Coroutine.
        }
        public void Initialize(AudioSO audioSO)
        {
            if (audioSource.isPlaying)
            {
                Debug.LogWarning($"n:{name}_initializing while playing audio");
                return;
            }

            currentAudioSO = audioSO;

            //global
            audioSource.clip = audioSO.clip;
            audioSource.outputAudioMixerGroup = audioSO.audioMixerGroup;

            //preplaced settings
            if(!overrideVolume)
                audioSource.volume = audioSO.Volume;
            if (isPrePlaced) return;

            audioSource.priority = audioSO.Priority;
            audioSource.pitch = audioSO.Pitch;
            audioSource.panStereo = audioSO.StreoPan;
            audioSource.spatialBlend = audioSO.GetSpatialBlend;
            audioSource.reverbZoneMix = audioSO.ReverbZoneMix;

            //3DSOUND SETTINGS
            audioSource.dopplerLevel = audioSO.DopplerLevel;
            audioSource.spread = audioSO.spread;
            audioSource.rolloffMode = audioSO.audioRolloffMode;
            audioSource.minDistance = audioSO.minDistance;
            audioSource.maxDistance = audioSO.maxDistance;
            if (audioSO.audioRolloffMode == AudioRolloffMode.Custom)
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioSO.curve);
        }
        /// <summary>
        /// call this when audio is stopped
        /// </summary>
        /// <param name="decraseDictionaryInstance"></param>
        private void OnAudioStop()
        {
            OnEndCallback?.Invoke();
            if(shouldDecraseCountOnDestroy)
                DecreaseDictionaryInstance(currentAudioSO);
            shouldDecraseCountOnDestroy = false;
        }
        private void OnDestroy()
        {
            if (!isInPool)// && shouldDecraseCountOnDestroy)//remove shdcod
            {
                //print("destroy runtime aud" + audioSource.clip.name);
                OnAudioStop();
            }

        }
    }
}
