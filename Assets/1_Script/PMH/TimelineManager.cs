using Swift_Blade.Feeling;
using UnityEngine;
using DG.Tweening;

namespace Swift_Blade.Timeline
{
    public class TimelineManager : MonoBehaviour
    {

        [SerializeField] private CameraShakeType ShakeType;
        [SerializeField] private Transform txt;
        [SerializeField] private Transform[] pillars;

        [SerializeField] private SceneManagerSO sceneManagerSo;
        
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
            //Debug.Log("´«¹°ÀÌ³ª");
            
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

        public void GoToBossScene(string sceneName)
        {
            sceneManagerSo.LoadScene(sceneName);
        }
        
    }
}
