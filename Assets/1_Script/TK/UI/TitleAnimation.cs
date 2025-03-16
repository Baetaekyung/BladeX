using System;
using UnityEngine;
using DG.Tweening;

namespace Swift_Blade
{
    public class TitleAnimation : MonoBehaviour
    {
        [SerializeField] private float animationScale;
        
        private void Start()
        {
            transform.DOShakeScale(1000f, Vector3.one * animationScale);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
