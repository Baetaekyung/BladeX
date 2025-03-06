using Swift_Blade.Audio;
using System;
using UnityEngine;

namespace Swift_Blade
{
    public class DefaultBreakableListener : MonoBehaviour
    {
        [SerializeField] private BreakableObject breakableObject;
        [SerializeField] private GameObject brokenModel;
        [SerializeField] private AudioEmitter audioEmitter;
        private void Awake()
        {
            breakableObject.OnDeadStart += OnDeadStart;
            breakableObject.OnGameObjectDestroy += OnGameObjectDestroy;
        }
        private void OnDeadStart(BreakableObject breakableObject)
        {
            brokenModel.SetActive(true);
            breakableObject.gameObject.SetActive(false);
            if(audioEmitter != null)
                audioEmitter.Play();
        }
        private void OnGameObjectDestroy(BreakableObject breakableObject)
        {
            Destroy(gameObject);
        }
    }
}
