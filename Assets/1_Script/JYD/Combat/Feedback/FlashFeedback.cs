using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade.Combat.Feedbck
{
    public class FlashFeedback : Feedback
    {
        [Header("Flash info")] 
        [SerializeField] private Transform root;
        
        [Range(0f,1f)][SerializeField] private float flashDuration;
        [Range(1,10)][SerializeField] private int flashCount;
        
        [SerializeField] private Material _flashMat;
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
        private Material[] _originMats;

        private void Start()
        {
            _meshRenderers = root.GetComponentsInChildren<SkinnedMeshRenderer>();
            _originMats = Array.ConvertAll(_meshRenderers, mesh => mesh.material);
        }

        public override void PlayFeedback()
        {
            StartCoroutine(FlashRoutine());
           
        }

        public override void ResetFeedback()
        {
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originMats[i];
            }
        }

        private IEnumerator FlashRoutine()
        {
            float waitTime = flashDuration / (flashCount * 2);
            
            for (int i = 0; i < flashCount; i++)
            {
                SetMaterials(_flashMat);
                yield return new WaitForSeconds(waitTime);
                ResetFeedback();
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void SetMaterials(Material mat)
        {
            foreach (var renderer in _meshRenderers)
            {
                renderer.material = mat;
            }
        }
    }
}