using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private string saveKey;
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField] private string mixerParamName;

        private float volume = 0;
        
        private void OnEnable()
        {
            volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
            volume = PlayerPrefs.GetFloat(saveKey, 50f);
            volumeSlider.value = volume;
        }

        private void HandleVolumeChanged(float value)
        {
            volume = value;
            mixerGroup.audioMixer.SetFloat(mixerParamName ,NormalizedValueToDb(volume));
        }
        
        private float NormalizedValueToDb(float vol)
        {
            return Mathf.Lerp(-80f, 0f, vol / 100f);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(saveKey, volume);
        }
    }
}
