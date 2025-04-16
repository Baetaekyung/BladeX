using UnityEngine;

namespace Swift_Blade
{
    public class RotateUI : MonoBehaviour
    {
        [SerializeField] private AnimationCurve rotateAnimationCurve;
        [SerializeField] private Vector3        rotatePivot;
        [SerializeField] private float          rotateSpeed;

        void Update()
        {
            int lastIndex       = rotateAnimationCurve.length - 1;
            float curveDuration = rotateAnimationCurve.keys[lastIndex].time;
            float curve         = rotateAnimationCurve.Evaluate(Time.time % curveDuration);

            //Do not mul vector first.
            transform.Rotate(rotatePivot * (rotateSpeed * curve * Time.deltaTime));
        }
    }
}
