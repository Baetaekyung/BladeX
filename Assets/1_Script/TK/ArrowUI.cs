using System;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class ArrowUI : MonoBehaviour
    {
        // [SerializeField] private float targetScale;
        // [SerializeField] private float animationSpeed;
        // private bool isBigger = false;
        //
        // private Sequence seq1;
        // private Sequence seq2;
        //
        // private void OnEnable()
        // {
        //     seq1 = DOTween.Sequence();
        //     seq2 = DOTween.Sequence();
        //     
        //     seq1.Append(transform.DOScaleX(-targetScale, 1 / animationSpeed));
        //     seq1.Join(transform.DOScaleY(targetScale, 1 / animationSpeed));
        //     
        //     seq2.Append(transform.DOScaleX(-1, 1 / animationSpeed));
        //     seq2.Join(transform.DOScaleY(1, 1 / animationSpeed));
        //
        //     seq1.OnComplete(() => seq2.Play().OnComplete(() => seq1.Play()));
        //
        //     seq1.Play();
        // }
        //
        // private void OnDisable()
        // {
        //     seq1.Kill();
        //     seq2.Kill();
        // }
    }
}
