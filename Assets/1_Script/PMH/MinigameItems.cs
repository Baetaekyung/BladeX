using UnityEngine;

namespace Swift_Blade
{
    public class MinigameItems : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private Transform VisualTrm;

        private float timeing = 0;

        private void OnEnable()
        {
            Destroy(gameObject, 5);
        }
        private void OnDestroy()
        {
            BallGenerator.Instance.RemoveMeInList(this.transform);
        }
        private void Update()
        {
            timeing += Time.deltaTime;

            float valueAnimCurv = animCurve.Evaluate(Mathf.Clamp((timeing / 5) % 1, 0, 1));
            transform.position += new Vector3(0, 0, -9) * (Time.deltaTime * valueAnimCurv * 3);

            VisualTrm.transform.Rotate(new Vector3(-60, 0, 0) * (Time.deltaTime * 5));
        }
    }
}
