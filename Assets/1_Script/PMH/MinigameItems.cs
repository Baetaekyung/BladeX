using UnityEngine;

namespace Swift_Blade
{
    public class MinigameItems : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private Transform VisualTrm;

        [SerializeField] private bool isMove = true;

        private float timeing = 0;

        private void OnEnable()
        {
            if(isMove) Destroy(gameObject, 5);
        }
        private void OnDestroy()
        {
            if(isMove)
            {
                BallGenerator.Instance.RemoveMeInList(this.transform);
            }
        }
        private void Update()
        {
            if (isMove)
            {
                VisualTrm.transform.Rotate(new Vector3(-60, 0, 0) * (Time.deltaTime * 5));
            }
            else
            {
                VisualTrm.transform.Rotate(new Vector3(0, 0, -60) * (Time.deltaTime * 5));
            }

            if (isMove == false) return;
            timeing += Time.deltaTime;

            float valueAnimCurv = animCurve.Evaluate(Mathf.Clamp((timeing / 5) % 1, 0, 1));
            transform.position += new Vector3(0, 0, -9) * (Time.deltaTime * valueAnimCurv * 3);

        }
    }
}
