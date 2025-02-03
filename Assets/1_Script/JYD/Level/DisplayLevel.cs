using UnityEngine;
using DG.Tweening;

namespace Swift_Blade
{
    public class DisplayLevel : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 2f;
        [SerializeField] private float moveDuration = 1f;

        void Start()
        {
            MoveUpDown();
        }

        void MoveUpDown()
        {
            transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}