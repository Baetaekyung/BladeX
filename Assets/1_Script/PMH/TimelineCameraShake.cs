using UnityEngine;
using DG.Tweening;
using Swift_Blade.Feeling;
using Unity.Cinemachine;

namespace Swift_Blade
{
    public class TimelineCameraShake : MonoBehaviour
    {

        [SerializeField] private CameraShakeType ShakeType;
        [SerializeField] private CinemachineCamera cam;

        [SerializeField] private Transform imagin, txt;

        [SerializeField] private Transform[] pillars;

        private void Awake()
        {
            foreach (Transform t in pillars)
            {
                t.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        public void CameraShakeCallback()
        {
            CameraShakeManager.Instance.DoShake(ShakeType);
            Debug.Log("´«¹°ÀÌ³ª");

            txt.DOMove(new Vector3(1404, 115, 0),0.8f);

            foreach(Transform t in pillars)
            {
                var compo = t.GetComponent<Rigidbody>();
                compo.useGravity = true;
                compo.freezeRotation = false;
                compo.mass = 1;
                compo.AddForce(new Vector3( -5, -5, 0) * 2, ForceMode.Impulse );
            }
        }
    }
}
