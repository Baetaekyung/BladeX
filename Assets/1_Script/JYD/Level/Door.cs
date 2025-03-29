using System.Collections;
using Swift_Blade.Feeling;
using Unity.Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade.Level.Door
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private NodeList nodeList;
        
        [SerializeField] private bool isDefaultPortal;
        
        [Range(0.1f , 10)] [SerializeField] private float delay;
        [Range(0.1f , 10)] [SerializeField] private float duration;
        
        [Space]
        
        [SerializeField] private CameraShakeType cameraShakeType;
        
        [SerializeField] private Transform door;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [SerializeField] private string sceneName;
        
        private void Start()
        {
            if (isDefaultPortal)
            {
                SetScene(nodeList.GetNodeName(NodeType.Exp));
            }
            else
            {
                Vector3 direction = Camera.main.transform.position - door.position;
                direction.y = 0; 
                door.rotation = Quaternion.LookRotation(direction);
            }

           
            //DOVirtual.DelayedCall(delay, (UpDoor()));
        }
        /*private void OnEnable()
        {
            cinemachineCamera.Priority = 1;
            DOVirtual.DelayedCall(delay, UpDoor);
        }

        private void OnDisable()
        {
            door.transform.position -= new Vector3(0,2.6f , 0);
        }*/

        public IEnumerator UpDoor(float doorMoveDelay = 0)
        {
            bool isFinished = false;
            
            cinemachineCamera.Priority = 1;
            
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() => CameraShakeManager.Instance.DoShake(cameraShakeType));
            sequence.Join(door.DOMoveY(transform.position.y + 0.25f, duration));
            sequence.AppendInterval(doorMoveDelay);
            sequence.AppendCallback(() => cinemachineCamera.Priority = -1);
            sequence.OnComplete(() => isFinished = true); 

            yield return new WaitUntil(() => isFinished);
        }
        
        public void Interact()
        {
            sceneManager.LoadScene(sceneName);
        }

        public void SetScene(string _sceneName)
        {
            sceneName = _sceneName;
        }
                
    }
}
