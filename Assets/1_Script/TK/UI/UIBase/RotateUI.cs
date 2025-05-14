using UnityEngine;

namespace Swift_Blade
{
    public class RotateUI : MonoBehaviour
    {
        [SerializeField] private AnimationCurve rotateAnimationCurve;
        [SerializeField] private Vector3        rotatePivot;
        [SerializeField] private float          rotateSpeed;

        private bool _isRotate = true;

        void Update()
        {
            if (_isRotate == false)
                return;

            int lastIndex       = rotateAnimationCurve.length - 1;
            float curveDuration = rotateAnimationCurve.keys[lastIndex].time;
            float curve         = rotateAnimationCurve.Evaluate(Time.time % curveDuration);

            //Do not mul vector first.
            transform.Rotate(rotatePivot * (rotateSpeed * curve * Time.deltaTime));
        }

        public void SetRotate(bool enable) => _isRotate = enable;
    }
}
