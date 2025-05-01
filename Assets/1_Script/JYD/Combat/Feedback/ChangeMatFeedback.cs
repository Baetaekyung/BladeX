using System;
using Swift_Blade.Enemy;
using UnityEngine;

namespace Swift_Blade.Combat.Feedback
{
    public class ChangeMatFeedback : Feedback
    {
        [Header("ChangeMat info")] 
        [SerializeField] private Transform root;
        
        [SerializeField] private Material changeMat;
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
        private Material[] _originMats;
        
        private BaseEnemy baseEnemy;
        private bool isChanging;
        private void Start()
        {
            _meshRenderers = root.GetComponentsInChildren<SkinnedMeshRenderer>();
            baseEnemy = root.GetComponent<BaseEnemy>();
            _originMats = Array.ConvertAll(_meshRenderers, mesh => mesh.material);
                        
            baseEnemy.OnSlowEvents.AddListener(ChangeMat);
        }
        
        private void OnDestroy()
        {
            baseEnemy.OnSlowEvents.RemoveListener(ChangeMat);
        }
        
        public bool GetChangingMat() => isChanging;
        
        public void ChangeMat(bool value)
        {
            isChanging = value;
            if (isChanging)
            {
                PlayFeedback();
            }
            else
            {
                ResetFeedback();
            }
        }
        
        public override void PlayFeedback()
        {
            SetMaterials(changeMat);
        }
        
        public override void ResetFeedback()
        {
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originMats[i];
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
